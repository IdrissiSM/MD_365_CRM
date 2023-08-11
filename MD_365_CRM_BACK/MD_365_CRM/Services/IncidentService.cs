using MD_365_CRM.CRM;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using MD_365_CRM.Services.IServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MD_365_CRM.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly DynamicsCRM _dynamicsCRM;
        private readonly string _baseUrl;

        public IncidentService(DynamicsCRM dynamicsCRM, IConfiguration configuration)
        {
            _dynamicsCRM = dynamicsCRM;
            _baseUrl = $"{configuration.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2";
        }

        public async Task<List<Incident>> GetAllIncidentsAsync()
        {
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

            if (incidents ==  null)
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

        public async Task<Incident> GetIncidentByIdAsync(Guid incidentId)
        {
            // get access token
            string accessToken = await _dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var response = await _dynamicsCRM.CrmRequest(
                HttpMethod.Get,
                accessToken,
            $"{_baseUrl}/incidents({incidentId})");

            var json = await response.Content.ReadAsStringAsync();
            var incident = JsonConvert.DeserializeObject<Incident>(json);

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
    }
}
