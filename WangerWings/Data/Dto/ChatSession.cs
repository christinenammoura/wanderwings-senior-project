namespace WangerWings.Data.Dto
{
    public class ChatSession
    {
        public Guid Id { get; set; }
        public bool? LikesQuiet { get; set; }
        public bool? LikesBeaches { get; set; }
        public int? Budget { get; set; }
        public string Stage { get; set; } = "start";
    }
}