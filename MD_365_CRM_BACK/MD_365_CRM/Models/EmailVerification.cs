using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Models
{
    public class EmailVerification
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
