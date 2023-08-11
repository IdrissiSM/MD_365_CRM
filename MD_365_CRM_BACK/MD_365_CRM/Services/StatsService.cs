using MD_365_CRM.CRM;
using MD_365_CRM.Models;
using MD_365_CRM.Responses;
using MD_365_CRM.Services.IServices;

namespace MD_365_CRM.Services
{
    public class StatsService : IStatsService
    {
        private readonly DynamicsCRM _dynamicsCRM;
        private readonly IAccountService _accountService;
        private readonly IOpportunityService _opportunityService;
        private readonly IIncidentService _incidentService;

        public StatsService(DynamicsCRM dynamicsCRM, IAccountService accountService,IOpportunityService opportunityService,IIncidentService incidentService)
        {
            _dynamicsCRM = dynamicsCRM;
            _accountService = accountService;
            _opportunityService = opportunityService;
            _incidentService = incidentService;
        }

        public async Task<List<OpportunityScore>> GetOpportunitiesScoreAsync(string month)
        {
            List<Opportunity> opportunities = await _opportunityService.GetOpportunities();

            List<OpportunityScore> opportunityScores = opportunities
                .Where(o => o.EstimatedCloseDate != null && o.CloseProbability != null &&  String.Format("{0:MM}", o.EstimatedCloseDate) == month)
                .Select(o => new OpportunityScore()
            {
                EstimatedCloseDate = o.EstimatedCloseDate,
                CloseProbability = o.CloseProbability
                
            }).ToList();

            return opportunityScores;

        }

        public async Task<List<OpportunityValues>> GetOpportunitiesValuesAsync()
        {
            List<Opportunity> opportunities = await _opportunityService.GetOpportunities();

            List<OpportunityValues> opportunitiesvalues = opportunities
                .GroupBy(o => String.Format("{0:MM}", o.CreatedOn))
                .Select(group => new OpportunityValues()
                {
                    EstimatedValue = group.Select(o => o.EstimatedValue).Sum(),
                    ActualValue = group.Select(o => o.ActualValue).Sum(),
                    month = group.Key

                }).ToList();

            return opportunitiesvalues;
        }

        public async Task<List<KeyValuePair<string, decimal>>> GetStatsAsync()
        {
            List<KeyValuePair<string, decimal>> stats = new List<KeyValuePair<string, decimal>>();
            //Calculate the nbr of customers
            List<Account> accounts = await _accountService.GetAllAccountsAsync();
            stats.Add(new KeyValuePair<string, decimal>(key: "Customers",value:accounts.Count()));

            //Calculate the nbr of opportunities
            List<Opportunity> opportunities = await _opportunityService.GetOpportunities();
            stats.Add(new KeyValuePair<string, decimal>(key: "Opportunities", value: opportunities.Count()));

            //Calculate total revenue
            decimal? revenue = accounts.Select(a => a.revenue).Sum();
            stats.Add(new KeyValuePair<string, decimal>(key: "Revenue", value: revenue ?? 0));

            //Calculate the nbr of incidents
            List<Incident> incidents = await _incidentService.GetAllIncidentsAsync();
            stats.Add(new KeyValuePair<string, decimal>(key: "Incidents", value: incidents.Count()));

            return stats;


        }


    }
}
