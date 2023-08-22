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
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace MD_365_CRM.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DynamicsCRM dynamicsCRM;
        private readonly IConfiguration config;
        private readonly ApplicationDbContext context;
        private readonly IBlacklistedUserService _blacklistedUserService;
        private readonly JWT _jwt;
        private readonly string baseUrl;


        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, DynamicsCRM crmConfig, IConfiguration config, ApplicationDbContext context, IBlacklistedUserService blacklistedUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            dynamicsCRM = crmConfig;
            this.config = config;
            this.context = context;
            _blacklistedUserService = blacklistedUserService;
            _jwt = jwt.Value;
            baseUrl = $"{config.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2";
        }

        /* TODO: Add contact id to claims */
        // TODO: Modify the registered user data in crm
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _blacklistedUserService.IsUserBlacklisted(request.Email))
                return new AuthResponse
                {
                    Message = "Registration Denied !",
                    IsAuthenticated = false,
                };
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
                return new AuthResponse
                {
                    Message = "65498Xsa", //Email is already registred !
                    IsAuthenticated = false,
                };

            var contact = await GetContactByEmail(request.Email);

            if (contact is null) return new AuthResponse
            {
                Message = "65269Lwd", // The registration process cannot proceed because the required resource has been deleted. Please contact an administrator for assistance
                IsAuthenticated = false,
            };

            User user = new()
            {
                Email = request.Email,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Jobtitle = request.Jobtitle,
                Gendercode = request.Gendercode,
                Statecode = request.Statecode,
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
            User user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return new AuthResponse()
                {
                    Message = "Credentials error: The given combination of email and password does not exist",
                    IsAuthenticated = false,
                };

            //if (!await _userManager.CheckPasswordAsync(user, request.Password))
            //{
            //    var errors = _userManager.PasswordValidators.Select(v => v.ValidateAsync(_userManager, user, request.Password).Result.Errors);

            //    return new AuthResponse()
            //    {
            //        Message = "Credentials error: " + (errors.First().IsNullOrEmpty() ? "The given combination of email and password does not exist" : string.Join("\n", errors.SelectMany(e => e.Select(err => err.Description)))),
            //        IsAuthenticated = false
            //    };
            //}

            var jwtSecurityToken = await CreateJwtToken(user);
            //var rolesList = await _userManager.GetRolesAsync(user);

            return new AuthResponse()
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
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

        
        public Otp IsOtpValid(string email, int otpValue)
        {
            Otp? otp = context.Otps.SingleOrDefault(otp => otp.Email == email);

            if (otp == null || otp.Value != otpValue || (DateTime.Now - otp.CreationDate).TotalMinutes >= config.GetValue<int>("Otp:validFor")) return null;

            return otp;
        }

        public bool UserExists(string email)
            => context.Users.Any(user => user.Email == email);
        public bool OtpExpired(string email)
            => (DateTime.Now - context.Otps.SingleOrDefault(otp => otp.Email == email)?.CreationDate)?.TotalMinutes >= config.GetValue<int>("Otp:validFor");

        public int CreateOtp(string email)
        {   // don't use this method unless you checked the authenticity of the email beforehand!
            var otp = RandomNumberGenerator.GetInt32(111111, 1000000);
            var outcasts = context.Otps.Where(otp => otp.Email == email);
            context.Otps.RemoveRange(outcasts);
            var secret = Guid.NewGuid().ToString("N");

            context.Otps.Add(new Otp()
            {
                CreationDate = DateTime.Now,
                Email = email,
                Value = otp,
                Secret = secret
            });

            return context.SaveChanges() > 0 ? otp: -1;
        }

        
        public int OtpValidFor() =>
            config.GetValue<int>("Otp:validFor");

        
        public async Task<AuthResponse> ResetPassword(ResetPasswordRequest request)
        {
            var passwordValidator = _userManager.PasswordValidators.First();

            var identityResult = await passwordValidator.ValidateAsync(_userManager, null, request.Password);

            if (!identityResult.Succeeded)
                return new AuthResponse()
                {
                    IsAuthenticated = false,
                    Message = identityResult.Errors.ToString()!
                };

            Contact? contact = await GetContactByEmail(request.Email);

            User? user = await _userManager.FindByEmailAsync(request.Email);

            if (contact is null || user is null || !IsSecretValid(request.Email, request.Secret))
                return new AuthResponse()
                {
                    IsAuthenticated = false,
                    Message = "It looks like either your account is no more, or the otp has expired"
                };

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.Password);

            if (result.Succeeded)
            {
                JwtSecurityToken jwt = await CreateJwtToken(user);

                return new AuthResponse
                {
                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwt)
                };
            }

            return new AuthResponse()
            {
                IsAuthenticated = false,
                Message = "500"
            };
                
        }

        
        public bool IsSecretValid(string email, string secret)
        {
            Otp? otp = context.Otps.SingleOrDefault(otp => otp.Email == email);

            if (otp == null || otp.Secret != secret || (DateTime.Now - otp.CreationDate).TotalMinutes >= config.GetValue<int>("Otp:validFor")) return false;

            return true;
        }



        public bool IsUserBlackListed(string email)
            => context.BlacklistedUsers.Any(user => user.Email == email);

        public async Task<Contact> UpdateProfile(UpdateProfileRequest updateProfile)
        {
            Contact? contact = await GetContactByEmail(updateProfile.Email);

            User? user = await _userManager.FindByEmailAsync(updateProfile.Email);

            if (contact is null || user == null)
                return null;

            if(!updateProfile.Firstname.IsNullOrEmpty())
                user.Firstname = updateProfile.Firstname;

            if(!updateProfile.Lastname.IsNullOrEmpty())
                user.Lastname = updateProfile.Lastname;

            if(!updateProfile.Username.IsNullOrEmpty())
                user.UserName = updateProfile.Username;

            if(!updateProfile.JobTitle.IsNullOrEmpty())
                user.Jobtitle = updateProfile.JobTitle;

            user.Gendercode = updateProfile.GenderCode;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return null;

            return new Contact()
            {
                firstname = user.Firstname,
                lastname = user.Lastname,
                fullname = user.UserName,
                emailaddress1 = user.Email,
                jobtitle = user.Jobtitle,
                gendercode = user.Gendercode,
                statuscode = user.Statecode,
                contactId = user.ContactId,
                secret = string.Empty,
            };
        }

        public async Task<APIResponse> ChangePassword(ChangePasswordRequest changePassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(changePassword.Email);

                if (user is null)
                {
                    return new APIResponse()
                    {
                        httpStatusCode = System.Net.HttpStatusCode.NotFound,
                        ErrorMessages = new List<string>() { "The user was not found" },
                        Success = false,
                    };
                }

                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, changePassword.OldPassword);

                if (!isPasswordCorrect) return new APIResponse()
                {
                    httpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string>() { "The given password is incorrect" },
                    Success = false,
                };

                var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);

                if (!result.Succeeded) return new APIResponse()
                {
                    httpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = result.Errors.Select(error => error.Description).ToList(),
                    Success = false,
                };

                return new APIResponse()
                {
                    httpStatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                }; ;
            }
            catch (Exception e)
            {
                // Log the exception for investigation
                Console.WriteLine($"Error during FindByEmailAsync: {e.Message}");

                return new APIResponse()
                {
                    httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ErrorMessages = new List<string>() { "An error occurred while processing your request" },
                    Success = false,
                };
            }
        }
    }
}
