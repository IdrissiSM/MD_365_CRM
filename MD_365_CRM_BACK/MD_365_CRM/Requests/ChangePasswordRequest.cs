using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Requests
{
    public class ChangePasswordRequest
    {
        [Required] public string OldPassword { get; set; }

        [Required] public string NewPassword { get; set; }

        [Required] public Guid ContactId { get; set; }

        [Required, EmailAddress] public string Email { get; set; }
    }
}
