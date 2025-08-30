using System.ComponentModel.DataAnnotations;

namespace WangerWings.Data.Dto
{
    public class UserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
