using MD_365_CRM.CRM;
using Newtonsoft.Json;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using Microsoft.Identity.Client;
using System.Security.Principal;
using MD_365_CRM.Services.IServices;

namespace MD_365_CRM.Services
{
    public class OpportunityService : IOpportunityService
    {
        private readonly DynamicsCRM dynamicsCRM;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;

        public OpportunityService(DynamicsCRM crmConfig, IConfiguration configuration)
        {
            _configuration = configuration;
            dynamicsCRM = crmConfig;
            baseUrl = $"{_configuration.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2";
        }

        public async Task<List<Opportunity>> GetOpportunities()
        {
            // get access token
            string accessToken = await dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var response = await dynamicsCRM.CrmRequest(
                HttpMethod.Get,
                accessToken,
                $"{baseUrl}/opportunities?" +
                $"$select={Opportunity.Properties}&" +
                $"$expand=product_opportunities" +
                $"($select={ProductOpportunity.Properties})");


            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            var Opportunities = result!.value.ToObject<List<Opportunity>>();

            return Opportunities;
        }

        public async Task<Opportunity> GetOpportunityById(Guid opportunityId)
        {
            // get access token
            string accessToken = await dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var response = await dynamicsCRM.CrmRequest(
                HttpMethod.Get,
                accessToken,
                $"{baseUrl}/opportunities({opportunityId})?" +
                $"$select={Opportunity.Properties}");

            var json = await response.Content.ReadAsStringAsync();
            var opportunity = JsonConvert.DeserializeObject<Opportunity>(json);

            return opportunity!;
        }
    }
}
