using MD_365_CRM.Models;
using MD_365_CRM.Requests;

namespace MD_365_CRM.Services.IServices
{
    public interface IOpportunityService
    {
        public Task<List<Opportunity>> GetOpportunities();
        public Task<Opportunity> GetOpportunityById(Guid opportunityId);
    }
}
