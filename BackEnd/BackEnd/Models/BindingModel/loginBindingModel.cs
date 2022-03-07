using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.BindingModel
{
    public class loginBindingModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
