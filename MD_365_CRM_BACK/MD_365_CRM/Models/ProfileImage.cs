using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Models
{
    public class ProfileImage
    {
        public int Id { get; set; }

        [MaxLength(4194304)]
        public byte[] ImageData { get; set; }
    }
}
