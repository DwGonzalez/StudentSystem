using BackEnd.Data.Entities;
using System;

namespace BackEnd.Models.DTO
{
    public class SubjectDTO
    {
        public SubjectDTO(int subjectId ,string subjectName, UserDTO professor, RoomDTO room)
        {
            SubjectId = subjectId;
            SubjectName = subjectName;
            Professor = professor;
            Room = room;
        }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public UserDTO Professor { get; set; }
        public RoomDTO Room { get; set; }
    }
}
