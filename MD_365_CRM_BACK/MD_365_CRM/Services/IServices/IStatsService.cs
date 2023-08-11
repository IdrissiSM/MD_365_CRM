using MD_365_CRM.Responses;

namespace MD_365_CRM.Services.IServices
{
    public interface IStatsService
    {
        Task<List<KeyValuePair<string, decimal>>> GetStatsAsync();
        Task<List<OpportunityValues>> GetOpportunitiesValuesAsync();
        Task<List<OpportunityScore>> GetOpportunitiesScoreAsync(string month);
    }
}
