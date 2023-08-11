using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Requests
{
    public class EmailVerificationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
