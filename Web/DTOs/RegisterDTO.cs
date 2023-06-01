using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class RegisterDTO
    {
        [MaxLength(15)]
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
    }
}
