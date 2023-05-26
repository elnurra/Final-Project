﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FinalProject.Models
{
    public class AppUser: IdentityUser
    {
        public string Fullname { get; set; } = null!;
        public bool IsActive { get; set; }

    }
}
