using BackEnd.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    public class AppDBContext: IdentityDbContext<AppUser, IdentityRole, string>
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<StudentSubject> StudentSubjects { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
    }
}
