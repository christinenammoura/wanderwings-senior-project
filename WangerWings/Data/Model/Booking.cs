using System.Text.Json.Serialization;

namespace WangerWings.Data.Model
{
    public class Booking
    {
        public int Id { get; set; } 
        public int userID { get; set; } 
        public int FlightID { get; set; }

        public Flight Flight { get; set; }

        public User User {  get; set; } 
    }
}
