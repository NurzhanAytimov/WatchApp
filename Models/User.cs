﻿using Microsoft.AspNetCore.Identity;

namespace WatchApp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
