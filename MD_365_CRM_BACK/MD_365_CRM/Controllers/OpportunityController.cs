using Microsoft.AspNetCore.Mvc;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using System.Security.Principal;
using MD_365_CRM.Responses;
using System.Net;
using MD_365_CRM.Services.IServices;

namespace MD_365_CRM.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OpportunityController : ControllerBase
    {
        private readonly IOpportunityService opportunityService;
        public OpportunityController(IOpportunityService opportunityService)
        {
            this.opportunityService = opportunityService;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetOpportunities()
        {
            APIResponse response = new();
            List<Opportunity> opportunitites = await opportunityService.GetOpportunities();
            if (opportunitites is null)
            {
                response.httpStatusCode = HttpStatusCode.NotFound;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "Opportunitites not found !"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = opportunitites;
            return Ok(response);
        }

        [HttpGet("{opportunitityId}")]
        public async Task<IActionResult> GetOpportunityById(Guid opportunitityId)
        {
            APIResponse response = new();
            Opportunity opportunitity = await opportunityService.GetOpportunityById(opportunitityId);
            if (opportunitity is null)
            {
                response.httpStatusCode = HttpStatusCode.NotFound;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    $"Opportunitity with ID {opportunitityId} not found !"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = opportunitity;
            return Ok(response);
        }
    }
}
