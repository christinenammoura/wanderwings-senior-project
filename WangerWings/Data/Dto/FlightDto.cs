namespace WangerWings.Data.Dto
{
    public class FlightDto
    {
        public required string Name { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public required DateTime FromDate { get; set; }
        public required DateTime TillDate { get; set; }
        public required double price { get; set; }
        
    }
}
