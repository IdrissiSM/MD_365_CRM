using MD_365_CRM.Models;
using MD_365_CRM.Responses;
using MD_365_CRM.Services;
using MD_365_CRM.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MD_365_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SynchronizeController : ControllerBase
    {
        private readonly ISynchronizeService _synchronizeService;
        public SynchronizeController(ISynchronizeService synchronizeService)
        {
            _synchronizeService = synchronizeService;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> SynchronizeAsync()
        {
            APIResponse response = new();
            var isSuccess = await _synchronizeService.SynchronizeAsync();
            if (!isSuccess)
            {
                response.httpStatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "Synchronization failed !"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = isSuccess;
            return Ok(response);
        } 
    }
}
