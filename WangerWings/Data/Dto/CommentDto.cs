using WangerWings.Data.Model;

namespace WangerWings.Data.Dto
{
    public class CommentDto
    {
        public string Email { get; set; }
        public required string Description { get; set; }
    }
}
