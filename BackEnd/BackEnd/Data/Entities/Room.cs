using System;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Data.Entities
{
    public class Room: BaseEntity
    {
        public string RoomName { get; set; }
    }
}
