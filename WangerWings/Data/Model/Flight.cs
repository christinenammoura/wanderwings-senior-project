namespace WangerWings.Data.Model
{
    public class Flight
    {
        public  int Id { get; set; } 
        public required string Name { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public required DateTime FromDate { get; set; }
        public required DateTime TillDate { get; set; }
        public required double price { get; set; }
        public required string FlightPicture { get; set; }

    }
}
