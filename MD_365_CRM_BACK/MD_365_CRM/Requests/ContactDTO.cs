using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Requests
{
    public class ContactDTO
    {
        public string? firstname { get; set; }
        [Key]
        public string? emailaddress1 { get; set; }
        public static string Properties { get; } = "firstname,emailaddress1";
    }
}
