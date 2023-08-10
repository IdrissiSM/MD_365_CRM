using MD_365_CRM.Context;
using MD_365_CRM.Models;
using MD_365_CRM.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace MD_365_CRM.Services
{
    public class BlacklistedUserService : IBlacklistedUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUsersService _usersService;

        public BlacklistedUserService(ApplicationDbContext applicationDbContext, IUsersService usersService) 
        {
            _applicationDbContext = applicationDbContext;
            _usersService = usersService;
        }
        public async Task<List<BlacklistedUser>> GetBlacklistedUsersAsync()
        {
            return await _applicationDbContext.BlacklistedUsers.ToListAsync();
        }
        public async Task<bool> IsUserBlacklisted(string email)
        {
            return await _applicationDbContext.BlacklistedUsers
                .AnyAsync(user => user.Email == email);
        }
        public async Task<bool> AddUserToBlacklist(string email)
        {
            if (!await IsUserBlacklisted(email))
            {
                var isDeleted = await _usersService.DeleteUserByEmailAsync(email);
                if (isDeleted)
                {
                    var user = new BlacklistedUser { Email = email };
                    _applicationDbContext.BlacklistedUsers.Add(user);
                    await _applicationDbContext.SaveChangesAsync();
                    return true;
                }
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveUserFromBlacklist(string email)
        {
            var user = await _applicationDbContext.BlacklistedUsers.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                _applicationDbContext.BlacklistedUsers.Remove(user);
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
