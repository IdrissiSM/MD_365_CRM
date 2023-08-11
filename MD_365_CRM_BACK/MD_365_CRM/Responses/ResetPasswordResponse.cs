using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MD_365_CRM.Responses
{
    public class ResetPasswordResponse
    {
        public bool IsReseted { get; set; }

        public string Message { get; set; }
    }
}
