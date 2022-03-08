using System;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Data.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
