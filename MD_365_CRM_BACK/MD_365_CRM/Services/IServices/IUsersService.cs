using MD_365_CRM.Models;

namespace MD_365_CRM.Services.IServices
{
    public interface IUsersService
    {
        Task<List<User>> GetUsersAsync();
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> DeleteUserByEmailAsync(string userEmail);
    }
}
