using MD_365_CRM.Models;
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
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService _incidentService;
        private APIResponse _response;

        public IncidentController(IIncidentService incidentService)
        {
            _incidentService = incidentService;
            _response = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetIncidents()
        {
            List<Incident> incidents = await _incidentService.GetAllIncidentsAsync();
            _response.httpStatusCode = HttpStatusCode.OK;
            _response.Result = incidents;
            return Ok(_response);
        }

        [HttpGet("{contactId:Guid}")]
        public async Task<ActionResult<APIResponse>> GetIncidents(Guid contactId)
        {

            //TODO : Check weather the contact exist or not
            List<Incident> incidents = await _incidentService.GetIncidentsAsync(contactId: contactId);
            _response.httpStatusCode = HttpStatusCode.OK;
            _response.Result = incidents;
            return Ok(_response);
        }

        [HttpGet("GetGroupedByContactIncidents")]
        public async Task<ActionResult<APIResponse>> GetGroupedByContactIncidents()
        {
            IEnumerable<Requests.GroupedIncidentsDTO> groupedIncidents = await _incidentService.GetGroupedByContactIncidentsAsync();

            _response.httpStatusCode = HttpStatusCode.OK;
            _response.Result = groupedIncidents;

            return Ok(_response);
        }


        [HttpGet("GetIncidentById/{incidentId:Guid}")]
        public async Task<ActionResult<APIResponse>> GetIncidentById(Guid incidentId)
        {
            Incident incident = await _incidentService.GetIncidentByIdAsync(incidentId);

            if (incident == null)
            {
                _response.Success = false;
                _response.httpStatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages = new List<string>()
                {
                    "Incident not found"
                };
                return NotFound(_response);
            }

            _response.httpStatusCode = HttpStatusCode.OK;
            _response.Result = incident;
            return Ok(_response);
        }
    }
}
