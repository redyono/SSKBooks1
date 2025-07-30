﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace SSKBooks.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string? FullName { get; set; }
    }
}
