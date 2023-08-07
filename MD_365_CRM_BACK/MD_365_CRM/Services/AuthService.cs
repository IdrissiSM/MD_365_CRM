using MD_365_CRM.CRM;
using MD_365_CRM.Helpers;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using MD_365_CRM.Responses;
using MD_365_CRM.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MD_365_CRM.Context;

namespace MD_365_CRM.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DynamicsCRM dynamicsCRM;
        private readonly IConfiguration config;
        private readonly ApplicationDbContext context;
        private readonly JWT _jwt;
        private readonly string baseUrl;


        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, DynamicsCRM crmConfig, IConfiguration config, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            dynamicsCRM = crmConfig;
            this.config = config;
            this.context = context;
            _jwt = jwt.Value;
            baseUrl = $"{config.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2";
        }

        /* TODO: Add contact id to claims */
        // TODO: Modify the registered user data in crm
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
                return new AuthResponse
                {
                    Message = "Email is already registred !",
                    IsAuthenticated = false,
                };

            var contact = await GetContactByEmail(request.Email);

            if (contact is null) return new AuthResponse
            {
                Message = "Access denied",
                IsAuthenticated = false,
            };

            User user = new()
            {
                Email = request.Email,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Jobtitle = request.Jobtitle,
                Gendercode = request.Gendercode != 0,
                Statecode = request.Statecode != 0,
                ContactId = contact.contactId,
                UserName = request.Username
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}\n";
                }
                return new AuthResponse
                {
                    Message = errors,
                    IsAuthenticated = false,
                };
            }

            await _userManager.AddToRoleAsync(user, "User");
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthResponse
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var authResponse = new AuthResponse();
            User user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                authResponse.Message = "Email or password is incorrect !";
                return authResponse;
            }


            var jwtSecurityToken = await CreateJwtToken(user);
            //var rolesList = await _userManager.GetRolesAsync(user);

            authResponse.IsAuthenticated = true;
            authResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return authResponse;
        }

        public async Task<string> AddRoleAsync(AddRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(request.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, request.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, request.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim("uid", user.Id),
                new Claim("contactid", user.ContactId.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return new AuthenticationProperties
            {
                RedirectUri = redirectUrl,
                Items =
            {
                { "LoginProvider", provider },
                { "XsrfKey", "1" },
            },
            };
        }

        public async Task<Contact> GetContactByEmail(string email)
        {
            // get access token
            string accessToken = await dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var response = await dynamicsCRM.CrmRequest(
                HttpMethod.Get,
                accessToken,
                $"{baseUrl}/contacts?$filter=emailaddress1 eq '{email}'");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            List<Contact> contact = result!.value.ToObject<List<Contact>>();

            Console.WriteLine(contact);
            return contact.FirstOrDefault();
        }

        public int GenerateOTP()
        {
            return RandomNumberGenerator.GetInt32(111111, 1000000);

        }

        public async Task SendEmail(string email, string body)
        {
            Console.WriteLine($"Email: {config.GetValue<string>("EmailService:email")}");
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(config.GetValue<string>("EmailService:email")));
            mail.To.Add(MailboxAddress.Parse(email));
            mail.Subject = "Email Confirmation";
            mail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(config.GetValue<string>("EmailService:host"), 587, SecureSocketOptions.StartTls);
            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            smtp.Authenticate(config.GetValue<string>("EmailService:email"), config.GetValue<string>("EmailService:password"));
            await smtp.SendAsync(mail);
            smtp.Disconnect(true);
        }

        public bool IsOtpValid(string email, int otpValue)
        {
            Otp? otp = context.Otps.SingleOrDefault(otp => otp.Email == email);

            if (otp == null || otp.Value != otpValue || (DateTime.Now - otp.CreationDate).TotalMinutes >= config.GetValue<int>("Otp:validFor")) return false;

            return true;
        }

        public bool CreateOtp(string email, int otp)
        {
            var outcasts = context.Otps.Where(otp => otp.Email == email);
            context.Otps.RemoveRange(outcasts);

            context.Otps.Add(new Otp()
            {
                CreationDate = DateTime.Now,
                Email = email,
                Value = otp
            });

            return context.SaveChanges() > 0;
        }

        public int OtpValidFor() =>
            config.GetValue<int>("Otp:validFor");
    }
}
