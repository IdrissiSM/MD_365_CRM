using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace MD_365_CRM.Requests
{
    public class AddProfileImageRequest
    {
        public string Email { get; set; }

        public int[] ImageData { get; set; }
    }
}
