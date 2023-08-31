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

        //[HttpGet("{contactId}")]
        //public async Task<ActionResult<APIResponse>> SynchronizeAsync(Guid contactId)
        //{
        //    APIResponse response = new();
        //    var isSuccess = await _synchronizeService.SynchronizeAsync(contactId);
        //    if (!isSuccess)
        //    {
        //        response.httpStatusCode = HttpStatusCode.BadRequest;
        //        response.Success = false;
        //        response.ErrorMessages = new List<string>()
        //        {
        //            "Synchronization failed !"
        //        };
        //        return BadRequest(response);
        //    }
        //    response.httpStatusCode = HttpStatusCode.OK;
        //    response.Success = true;
        //    response.Result = isSuccess;
        //    return Ok(response);
        //}

        [HttpGet("incidents/{contactId}")]
        public async Task<ActionResult<APIResponse>> SynchronizeIncidentsAsync(Guid contactId)
        {
            APIResponse response = new();
            var isSuccess = await _synchronizeService.SynchronizeIncidentsAsync(contactId);
            if (!isSuccess)
            {
                response.httpStatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "Incidents synchronization failed !"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = isSuccess;
            return Ok(response);
        }

        [HttpGet("opportunities/{contactId}")]
        public async Task<ActionResult<APIResponse>> SynchronizeOpportunitiesAsync(Guid contactId)
        {
            APIResponse response = new();
            var isSuccess = await _synchronizeService.SynchronizeOpportunitiesAsync(contactId);
            if (!isSuccess)
            {
                response.httpStatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "Opportunities synchronization failed !"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = isSuccess;
            return Ok(response);
        }

        [HttpGet("Products/{contactId}")]
        public async Task<ActionResult<APIResponse>> SynchronizeProductsAsync(Guid contactId)
        {
            APIResponse response = new();
            var isSuccess = await _synchronizeService.SynchronizeProductsAsync(contactId);
            if (!isSuccess)
            {
                response.httpStatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "Products synchronization failed !"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = isSuccess;
            return Ok(response);
        }

        [HttpGet("profile/{contactId}")]
        public async Task<ActionResult<APIResponse>> SynchronizeProfileAsync(Guid contactId)
        {
            APIResponse response = new();
            var isSuccess = await _synchronizeService.SynchronizeProfileAsync(contactId);
            if (!isSuccess)
            {
                response.httpStatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "Profile synchronization failed !"
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
