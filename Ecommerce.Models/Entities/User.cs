using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public String Role { get; set; }
        public DateTime CraetedAt { get; set; } = DateTime.UtcNow;
    }
}
