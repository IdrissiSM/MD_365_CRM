using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Requests
{
    public class AddRoleRequest
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
