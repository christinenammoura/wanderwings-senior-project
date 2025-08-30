using Microsoft.EntityFrameworkCore;
using WangerWings.Data.Model;

namespace WangerWings.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Flight> Flights { get; set; }  
        public DbSet<Booking> Bookings { get; set; }    
        public DbSet<Destinations> Destinations { get; set; }
        public DbSet<Comment> Comments { get; set; }    
    }
}
