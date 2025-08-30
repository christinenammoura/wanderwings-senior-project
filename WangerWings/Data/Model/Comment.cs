using System.Globalization;

namespace WangerWings.Data.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserID { get; set; }  
        public required string Description {  get; set; } 
        public DateTime CreatedDate { get; set; }   
        public User User { get; set; }

    }
}
