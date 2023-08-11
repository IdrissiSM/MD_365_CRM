using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Requests
{
    public class EmailConfirmationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int Otp { get; set; }
    }
}
