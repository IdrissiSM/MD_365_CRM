using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Requests
{
    public class BlacklistedUserRequest
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
