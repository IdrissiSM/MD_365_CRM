using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using MD_365_CRM.Responses;
using Microsoft.AspNetCore.Authentication;

namespace MD_365_CRM.Services.IServices
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest model);
        Task<AuthResponse> LoginAsync(LoginRequest model);
        public Task<string> AddRoleAsync(AddRoleRequest model);
        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
    }
}
