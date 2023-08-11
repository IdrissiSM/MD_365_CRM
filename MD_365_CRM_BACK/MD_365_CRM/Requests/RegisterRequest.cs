﻿using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Requests
{
    public class RegisterRequest
    {
        [Required, StringLength(100)]
        public string Firstname { get; set; }

        [Required, StringLength(100)]
        public string Lastname { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(128), EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }

        public string Jobtitle { get; set; }

        public int Gendercode { get; set; }

        public int Statecode { get; set; }

        [Required]
        public string Secret { get; set; }
    }
}
