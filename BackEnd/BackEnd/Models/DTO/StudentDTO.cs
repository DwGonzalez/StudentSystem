using System;
using System.Collections.Generic;

namespace BackEnd.Models.DTO
{
    public class StudentDTO
    {
        public StudentDTO(string fullName, string email, string userName, DateTime dateCreated)
        {
            FullName = fullName;
            Email = email;
            UserName = userName;
            DateCreated = dateCreated;
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public List<string> Subjects { get; set; }
    }
}
