using Microsoft.AspNetCore.Mvc;
using MD_365_CRM.CRM;
using MD_365_CRM.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MD_365_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMAuthController : ControllerBase
    {
        private readonly DynamicsCRM dynamicsCRM;

        public CRMAuthController(DynamicsCRM crmConfig, IConfiguration configuration)
        {
            dynamicsCRM = crmConfig;
        }


        [HttpGet("DynamicsCRMAuthentication")]
        public async Task<IActionResult> Auth()
        {
            return Ok(await dynamicsCRM.GetAccessTokenAsync());
        }
    }
}
