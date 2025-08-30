using System.ComponentModel.DataAnnotations;

namespace WangerWings.Data.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; }
    }
}
