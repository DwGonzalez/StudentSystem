using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Data.Entities
{
    public class Subject: BaseEntity
    {
        public string ClassName { get; set; }
        public string ProfessorId { get; set; }
        [ForeignKey("ProfessorId")]
        public virtual AppUser Professor { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
    }
}
