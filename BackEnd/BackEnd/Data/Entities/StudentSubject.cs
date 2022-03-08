using System.ComponentModel.DataAnnotations;

namespace BackEnd.Data.Entities
{
    public class StudentSubject
    {
        [Key]
        public int Id { get; set; }
        public string StudentId { get; set; }
        public virtual AppUser Student { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
