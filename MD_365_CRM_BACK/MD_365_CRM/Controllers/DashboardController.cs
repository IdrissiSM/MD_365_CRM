using MD_365_CRM.Responses;
using MD_365_CRM.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MD_365_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IStatsService _statsService;
        private readonly APIResponse _response;

        public DashboardController(IStatsService statsService)
        {
            _statsService = statsService;
            _response = new APIResponse();
        }

        [HttpGet("GetStats")]
        public async Task<ActionResult<APIResponse>> GetStats()
        {
            List<KeyValuePair<string, decimal>> stats = await _statsService.GetStatsAsync();

            _response.httpStatusCode = HttpStatusCode.OK;
            _response.Result = stats;

            return Ok(_response);
            
        }

        [HttpGet("GetOpportunitiesValues")]
        public async Task<ActionResult<APIResponse>> GetOpportunitiesValues()
        {
            List<OpportunityValues> opportunitiesValues = await _statsService.GetOpportunitiesValuesAsync();

            _response.httpStatusCode = HttpStatusCode.OK;
            _response.Result = opportunitiesValues;

            return Ok(_response);
        }


        [HttpGet("GetOpportunitiesScoreByMonth/{month}")]
        public async Task<ActionResult<APIResponse>> GetOpportunitiesScore(string month)
        {
            List<OpportunityScore> opportunityScores = await _statsService.GetOpportunitiesScoreAsync(month);

            _response.httpStatusCode = HttpStatusCode.OK;
            _response.Result = opportunityScores;

            return Ok(_response);
        }

    }
}
