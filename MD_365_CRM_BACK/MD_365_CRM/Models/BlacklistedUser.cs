using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Models
{
    public class BlacklistedUser
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
