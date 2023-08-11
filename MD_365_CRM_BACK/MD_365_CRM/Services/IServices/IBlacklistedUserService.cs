using MD_365_CRM.Models;

namespace MD_365_CRM.Services.IServices
{
    public interface IBlacklistedUserService
    {
        Task<List<BlacklistedUser>> GetBlacklistedUsersAsync();
        Task<bool> IsUserBlacklisted(string email);
        Task<bool> AddUserToBlacklist(string email);
        Task<bool> RemoveUserFromBlacklist(string email);
    }
}
