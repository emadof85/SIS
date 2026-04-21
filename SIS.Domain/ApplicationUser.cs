using Microsoft.AspNetCore.Identity;
using System;

namespace SIS.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}