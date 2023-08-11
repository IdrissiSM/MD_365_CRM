using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using MD_365_CRM.Responses;

namespace MD_365_CRM.Services.IServices
{
    public interface IAuthService
    {
        Task<string> AddRoleAsync(AddRoleRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest model);
        Task<AuthResponse> LoginAsync(LoginRequest model);
        public Task<Contact> GetContactByEmail(string email);
        Task SendEmail(string email, string body);
        Otp IsOtpValid(string email, int otp);
        int CreateOtp(string email);
        int OtpValidFor();
        Task<AuthResponse> ResetPassword(ResetPasswordRequest request);
        bool IsSecretValid(string email, string secret);
    }
}
