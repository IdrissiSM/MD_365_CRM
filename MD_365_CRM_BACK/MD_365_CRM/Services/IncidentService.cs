using MD_365_CRM.Context;
using MD_365_CRM.CRM;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using MD_365_CRM.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MD_365_CRM.Services
{
    public class IncidentService : IIncidentService 
    {
        private readonly DynamicsCRM _dynamicsCRM;
        private readonly string _baseUrl;
        private readonly ApplicationDbContext _dbContext;

        public IncidentService(DynamicsCRM dynamicsCRM,IConfiguration configuration, ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;
            _dynamicsCRM = dynamicsCRM;
            _baseUrl = $"{configuration.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2";
        }

        public async Task<List<Incident>> GetAllIncidentsAsync()
        {

            var AllRecords = _dbContext.Incidents.ToList();
            if (AllRecords.Count() > 0)
            {
                return AllRecords;
            }

            // get access token
            string accessToken = await _dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var response = await _dynamicsCRM.CrmRequest(
                HttpMethod.Get,
                accessToken,
                $"{_baseUrl}/incidents");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            var incidents = result!.value.ToObject<List<Incident>>();

            if (incidents is not null)
            {
                foreach (Incident incident in incidents)
                {
                    _dbContext.Incidents.AddAsync(incident);
                }
                _dbContext.SaveChangesAsync();
            }

            return incidents;
        }

        public async Task<Incident> GetIncidentByIdAsync(Guid incidentId)
        {
            Incident incident = await _dbContext.Incidents.FindAsync(incidentId);
            return incident!;
        }

        public async Task<List<Incident>> GetIncidentsAsync(Guid contactId)
        {
            // get access token
            string accessToken = await _dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var response = await _dynamicsCRM.CrmRequest(
                HttpMethod.Get,
                accessToken,
                $"{_baseUrl}/incidents?$filter=_customerid_value eq {contactId}&$select={Incident.Properties()}");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            var incidents = result!.value.ToObject<List<Incident>>();

            return incidents;
        }

        private async Task<List<Incident>> GetAllIncidents2Async()
        {
            // get access token
            string accessToken = await _dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var response = await _dynamicsCRM.CrmRequest(
               HttpMethod.Get,
               accessToken,
               $"{_baseUrl}/incidents?" +
               $"$select={Incident.Properties()}&" +
               $"$expand=customerid_contact" +
               $"($select={ContactDTO.Properties})");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            var incidents = result!.value.ToObject<List<Incident>>();

            return incidents;
        }

        public async Task<IEnumerable<GroupedIncidentsDTO>> GetGroupedByContactIncidentsAsync()
        {
            List<Incident> incidents = await GetAllIncidents2Async();

            if (incidents == null)
            {
                throw new Exception("Incidents is null");
            }

            IEnumerable<GroupedIncidentsDTO> groupedIncident = incidents.GroupBy(i => i.customerid_contact.firstname)
                .Select(group => new GroupedIncidentsDTO()
                {
                    Name = group.Key,
                    Count = group.Count()
                });

            return groupedIncident.Take(4);
        }

        public async Task<bool> UpdateIncident(Guid IncidentId, IncidentRequest incident)
        {
            // get access token
            string accessToken = await _dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var jsonIncident = JsonConvert.SerializeObject(incident);
            var response = await _dynamicsCRM.CrmRequest(
                HttpMethod.Patch,
                accessToken,
                $"{_baseUrl}/incidents({IncidentId})",
                jsonIncident);

            return response.IsSuccessStatusCode;
        }
    }
}
