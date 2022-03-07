using Microsoft.AspNetCore.Identity;
using System;

namespace BackEnd.Models
{
    public class Users: IdentityUser
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
