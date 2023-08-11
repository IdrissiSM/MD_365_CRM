using MD_365_CRM.CRM;
using MD_365_CRM.Models;
using MD_365_CRM.Services.IServices;
using Newtonsoft.Json;

namespace MD_365_CRM.Services
{
    public class AccountService : IAccountService
    {
        private readonly DynamicsCRM _dynamicsCRM;
        private readonly string _baseUrl;

        public AccountService(DynamicsCRM dynamicsCRM,IConfiguration configuration)
        {
            _dynamicsCRM = dynamicsCRM;
            _baseUrl = $"{configuration.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2";
        }
        public async Task<List<Account>> GetAllAccountsAsync()
        {
            // get access token
            string accessToken = await _dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var response = await _dynamicsCRM.CrmRequest(
                HttpMethod.Get,
                accessToken,
                $"{_baseUrl}/accounts");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            var accounts = result!.value.ToObject<List<Account>>();

            return accounts;
        }

        public async Task<List<Account>> GetAllAcountsOrderedByRevenueAsync()
        {
            List<Account> accounts = await GetAllAccountsAsync();

            if (accounts == null)
            {
                return null;
            }

            List<Account> orderedAccounts = accounts.OrderByDescending(a => a.revenue).ToList();

            return orderedAccounts;
        }
    }
}
