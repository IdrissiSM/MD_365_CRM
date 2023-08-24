using MD_365_CRM.CRM;
using Newtonsoft.Json;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using Microsoft.Identity.Client;
using System.Security.Principal;
using MD_365_CRM.Services.IServices;
using MD_365_CRM.Context;
using Microsoft.EntityFrameworkCore;

namespace MD_365_CRM.Services
{
    public class OpportunityService : IOpportunityService
    {
        private readonly DynamicsCRM dynamicsCRM;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly ApplicationDbContext _dbContext;

        public OpportunityService(DynamicsCRM crmConfig, IConfiguration configuration, ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;
            _configuration = configuration;
            dynamicsCRM = crmConfig;
            baseUrl = $"{_configuration.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2";
        }

        public async Task<List<Opportunity>> GetOpportunities()
        {
            var AllRecords = _dbContext.Opportunities
                .Include(op => op.Product_opportunities)
                .ToList();
            if (AllRecords.Count() > 0)
            {
                return AllRecords;
            }

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

            if(Opportunities is not null)
            {
                foreach (Opportunity opp in Opportunities)
                {
                    _dbContext.Opportunities.AddAsync(opp);
                }
                _dbContext.SaveChangesAsync();
            }
            return Opportunities;
        }

        public async Task<bool> UpdateOpportunity(Guid opportunityId, OpportunityRequest opportunity)
        {
            // get access token
            string accessToken = await dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var jsonOpportunity = JsonConvert.SerializeObject(opportunity);
            var response = await dynamicsCRM.CrmRequest(
                HttpMethod.Patch,
                accessToken,
                $"{baseUrl}/opportunities({opportunityId})",
                jsonOpportunity);

            return response.IsSuccessStatusCode;
        }

        public async Task<Opportunity> GetOpportunityById(Guid opportunityId)
        {
            Opportunity opportunity = await _dbContext.Opportunities.FindAsync(opportunityId);
            return opportunity!;
        }
    }
}
