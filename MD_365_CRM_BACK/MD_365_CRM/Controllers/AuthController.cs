using Microsoft.AspNetCore.Mvc;
using MD_365_CRM.Requests;
using MD_365_CRM.Services.IServices;
using MD_365_CRM.Models;


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
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            return Ok(result);
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
        [ProducesResponseType(200, Type = typeof(void))]
        public async Task<IActionResult> EmailVerification([FromBody] EmailVerification emailVerification)
        {

            if (!ModelState.IsValid || emailVerification == null || emailVerification.Email == "")
                return BadRequest(ModelState);

            Contact contact = await _authService.GetContactByEmail(emailVerification.Email);

            if (contact is null)
            {
                ModelState.AddModelError("", "Uh Oh! It sounds like that email of yours is not eligible");
                return StatusCode(403, ModelState);
            }

            int storedOtp = _authService.GenerateOTP();

            await _authService.SendEmail(emailVerification.Email, $@"
                <div style=""font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2"">
                    <div style=""text-align: center; margin:50px auto;width:70%;padding:20px 0"">
                        <div style=""border-bottom:1px solid #eee"">
                            <a href="""" style=""font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600"">Mobs here!</a>
                        </div>
                        <img src=""https://office24by7.com/assets/img/Email.png"" alt=""Mail Svg Img"" height=""250"">
                        <p style=""font-size:1.1em"">Hi,</p>
                        <p>To complete your Sign Up procedures. This code is valid for {_authService.OtpValidFor()} minutes</p>
                        <h2 style=""letter-spacing: 2px;background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;"">{storedOtp}</h2>
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

            _authService.CreateOtp(contact.emailaddress1, storedOtp);

            return Ok();

        }

        [HttpPost("email_confirmation")]
        [ProducesResponseType(200, Type = typeof(Contact))]
        public async Task<IActionResult> EmailConfirmation([FromBody] EmailConfirmation emailConfirmation)
        {
            if (!ModelState.IsValid || emailConfirmation.Otp < 111111 || emailConfirmation.Otp > 999999)
                return BadRequest(ModelState);

            Contact contact = await _authService.GetContactByEmail(emailConfirmation.Email);

            if (contact == null)
            {
                ModelState.AddModelError("", "Uh Oh! Something went really wrong while processing your request");
                return StatusCode(500, ModelState);
            }
            // null will be returned in one and only scenario: when the otp is not valid. hence the front can handle it.
            Console.WriteLine("otp is valid? " + _authService.IsOtpValid(contact.emailaddress1, emailConfirmation.Otp));
            return Ok(_authService.IsOtpValid(contact.emailaddress1, emailConfirmation.Otp) ? contact : null);
        }


    }
}
