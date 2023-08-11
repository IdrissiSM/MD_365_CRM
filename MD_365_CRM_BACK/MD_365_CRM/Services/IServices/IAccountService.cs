using MD_365_CRM.Models;

namespace MD_365_CRM.Services.IServices
{
    public interface IAccountService
    {
        Task<List<Account>> GetAllAccountsAsync();
        Task<List<Account>> GetAllAcountsOrderedByRevenueAsync();
    }
}
