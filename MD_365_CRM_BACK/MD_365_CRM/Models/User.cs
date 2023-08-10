using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Models
{
    public class User : IdentityUser
    {
        public Guid ContactId { get; set; }
        [RegularExpression(@"^[A-Za-z]+( [A-Za-z]+)?( [A-Za-z]+)?$", ErrorMessage = "Invalid first name format.")]
        public string Firstname { get; set; } = String.Empty;
        [RegularExpression(@"^[A-Za-z]+( [A-Za-z]+)?( [A-Za-z]+)?$", ErrorMessage = "Invalid last name format.")]
        public string Lastname { get; set; } = String.Empty;
        [RegularExpression(@"^[A-Za-z]+( [A-Za-z]+)?( [A-Za-z]+)?( [A-Za-z]+)?$", ErrorMessage = "Invalid jobtitle format.")]
        public string? Jobtitle { get; set; }
        public int? Gendercode { get; set; }
        public int? Statecode { get; set; }
    }
}