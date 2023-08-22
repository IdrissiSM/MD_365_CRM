using Microsoft.AspNetCore.Mvc;
using MD_365_CRM.Requests;
using MD_365_CRM.Services.IServices;
using MD_365_CRM.Models;
using MD_365_CRM.Responses;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Win32;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MD_365_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        // TODO: Modify the registered user data in crm
        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(AuthResponse))]
        [ProducesResponseType(400, Type = typeof(AuthResponse))]
        [ProducesResponseType(403, Type = typeof(AuthResponse))]
        [ProducesResponseType(409, Type = typeof(AuthResponse))]

        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest register)
        {
            if (!ModelState.IsValid)
            {
                string errorsString = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(new AuthResponse()
                {
                    Message = errorsString,
                    IsAuthenticated = false
                });
            }

            if (!_authService.IsSecretValid(register.Email, register.Secret))
            {
                return StatusCode(400, new AuthResponse()
                {
                    Message = "The registration session has expired. Please request a new OTP and try again.",
                    IsAuthenticated = false
                });
            }

            var result = await _authService.RegisterAsync(register);

            if(!result.IsAuthenticated)
            {
                if(result.Message == "65498Xsa")
                {
                    result.Message = "Email is already registred !";
                    return StatusCode(409, result);
                }

                else if(result.Message == "65269Lwd")
                {
                    result.Message =  "The registration process cannot proceed because the required resource has been deleted. Please contact an administrator for assistance";
                    return StatusCode(403, result);
                }
                else
                {
                    result.Message =  result.Message;
                    return StatusCode(400, result);
                }

            }

            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(AuthResponse))]
        [ProducesResponseType(400, Type = typeof(AuthResponse))]
        [ProducesResponseType(401, Type = typeof(AuthResponse))]
        [ProducesResponseType(500, Type = typeof(AuthResponse))]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                string errorsString = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(new AuthResponse()
                {
                    Message = errorsString,
                    IsAuthenticated = false
                });
            }

            var result = await _authService.LoginAsync(model);

            if(result.IsAuthenticated)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("role")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpPost("email_verification")]
        [ProducesResponseType(200, Type = typeof(EmailVerificationResponse))]
        public async Task<IActionResult> EmailVerification([FromBody] EmailVerificationRequest emailVerification)
        {

            if (!ModelState.IsValid)
            {
                string errorsString = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(new EmailVerificationResponse()
                {
                    Message = errorsString,
                    IsEligible = false
                });
            }

            if(emailVerification.Registering && _authService.UserExists(emailVerification.Email))
                return BadRequest(new EmailVerificationResponse()
                {
                    Message = "There is already an account with the given email",
                    IsEligible = false,
                    HasAccount = true,
                });

            if(!emailVerification.Registering && !_authService.UserExists(emailVerification.Email))
                return BadRequest(new EmailVerificationResponse()
                {
                    Message = "There is no account with such an email.",
                    IsEligible = false,
                    HasAccount = false,
                });

            Contact contact = await _authService.GetContactByEmail(emailVerification.Email);

            if (contact is null || _authService.IsUserBlackListed(emailVerification.Email))
                return StatusCode(403, new EmailVerificationResponse()
                {
                    Message = "The provided email does not meet the eligibility criteria for creating an account.",
                    IsEligible = false
                });

            int otp = _authService.CreateOtp(contact.emailaddress1);

            if(otp == -1)
            {
                return StatusCode(500, new EmailVerificationResponse()
                {
                    Message = "There seems to be an internal issue while processing your request. Please understand that our system is experiencing technical difficulties at the moment. We kindly ask you to consider trying again later.",
                    IsEligible = false
                });
            }

            await _authService.SendEmail(emailVerification.Email, $@"
                <div style=""font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2"">
                    <div style=""text-align: center; margin:50px auto;width:70%;padding:20px 0"">
                        <div style=""border-bottom:1px solid #eee"">
                            <a href="""" style=""font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600"">Mobs here!</a>
                        </div>
                        <img src=""https://office24by7.com/assets/img/Email.png"" alt=""Mail Svg Img"" height=""250"">
                        <p style=""font-size:1.1em"">Hi,</p>
                        <p>To complete your Sign Up procedures. This code is valid for {_authService.OtpValidFor()} minutes</p>
                        <h2 style=""letter-spacing: 2px;background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;"">{otp}</h2>
                        <p style=""font-size:0.9em;"">Regards,<br />Mobs</p>
                        <hr style=""border:none;border-top:1px solid #eee"" />
                        <div style=""float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300"">
                            <p>Mobs Inc</p>
                            <p>1600 Amphitheatre Parkway</p>
                            <p>California</p>
                        </div>
                    </div>
                </div>
            ");

            return Ok();

        }

  
        
        [HttpPost("email_confirmation")]
        [ProducesResponseType(200, Type = typeof(Contact))]
        public async Task<IActionResult> EmailConfirmation([FromBody] EmailConfirmationRequest emailConfirmation)
        {
            if (!ModelState.IsValid)
            {
                string errorsString = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(new AuthResponse()
                {
                    Message = errorsString,
                    IsAuthenticated = false
                });
            }

            if (emailConfirmation.Otp < 111111 || emailConfirmation.Otp > 999999)
                return BadRequest(new AuthResponse() {
                    Message = "The 'otp' field must be a value between 111111 and 999999.",
                    IsAuthenticated = false
                });

            Contact contact = await _authService.GetContactByEmail(emailConfirmation.Email);

            if (contact == null)
                return BadRequest(new AuthResponse()
                {
                    Message = "The registration process cannot proceed because the required resource has been deleted. Please contact an administrator for assistance",
                    IsAuthenticated = false
                });

            //if (_authService.OtpExpired(emailConfirmation.Email))
            //    return new AuthResponse()
            //    {
            //        IsAuthenticated = false,
            //        Message = "Your otp has expired"
            //    };

            Otp otp = _authService.IsOtpValid(contact.emailaddress1, emailConfirmation.Otp);

            if (otp is null)
            {
                ModelState.AddModelError("", "The given Otp is not valid. maybe you should request a new one.");
                return StatusCode(400, ModelState);
            }

            contact.secret = otp.Secret;

            return Ok(contact);
        }

        [HttpPost("reset_password")]
        [ProducesResponseType(200, Type = typeof(ResetPasswordResponse))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                string errorsString = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(new ResetPasswordResponse()
                {
                    IsReseted = false,
                    Message = errorsString
                });
            }

            if (!_authService.IsSecretValid(resetPasswordRequest.Email, resetPasswordRequest.Secret))
            {
                return StatusCode(400, new AuthResponse()
                {
                    Message = "The registration session has expired. Please request a new OTP and try again.",
                    IsAuthenticated = false
                });
            }

            AuthResponse authState = await _authService.ResetPassword(resetPasswordRequest);

            if(authState.IsAuthenticated)
            {
                return Ok(new ResetPasswordResponse()
                {
                    IsReseted = true
                });
            }

            if (authState.Message == "500")
                return StatusCode(500, new ResetPasswordResponse()
                {
                    IsReseted = false,
                    Message = "The reset failed due to some internal reasons"
                });

            return StatusCode(400, new ResetPasswordResponse()
            {
                IsReseted = false,
                Message = authState.Message
            });

        }

        [HttpPost("update_user_profile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(304)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Contact>> UpdateProfile([FromBody] UpdateProfileRequest updateProfile)
        {
            Contact? contact = await _authService.UpdateProfile(updateProfile);

            if (contact is null)
                return StatusCode(500, "Uh Oh! Something went really wrong while updating your profile");

            return Ok(contact);
        }
    }
}
