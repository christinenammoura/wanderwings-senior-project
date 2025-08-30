using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WangerWings.Data.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [JsonIgnore]
        public required byte[] PasswordHash { get; set; } 
        [JsonIgnore]
        public required byte[] PasswordSalt { get; set; }
        public required string ProfilePicture { get; set; }
    }
}
