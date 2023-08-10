using MD_365_CRM.Context;
using MD_365_CRM.Models;
using MD_365_CRM.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MD_365_CRM.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersService(ApplicationDbContext applicationDbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<User>> GetUsersAsync()
        {
            var adminRole = await _roleManager.FindByNameAsync("Admin");

            var adminUsers = await _userManager.GetUsersInRoleAsync(adminRole.Name);

            return await _applicationDbContext.Users
                .Where(u => !adminUsers.Contains(u))
                .ToListAsync();
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _applicationDbContext.Users.FindAsync(userId);

            if (user != null)
            {
                _applicationDbContext.Users.Remove(user);
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteUserByEmailAsync(string userEmail)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user != null)
            {
                _applicationDbContext.Users.Remove(user);
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
