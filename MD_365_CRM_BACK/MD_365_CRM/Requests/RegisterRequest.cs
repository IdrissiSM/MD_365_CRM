using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Requests
{
    public class RegisterRequest
    {
        [Required, StringLength(100)]
        public string Firstname { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Lastname { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, StringLength(128)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(256)]
        public string Password { get; set; } = string.Empty;
    }
}
