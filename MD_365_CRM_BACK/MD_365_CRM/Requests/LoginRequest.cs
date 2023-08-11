﻿using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Requests
{
    public class LoginRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
