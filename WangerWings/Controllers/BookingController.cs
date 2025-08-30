using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop.Infrastructure;
using WangerWings.Data;
using WangerWings.Data.Dto;
using WangerWings.Data.Model;

namespace WangerWings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        public BookingController(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        [HttpPost("Book")]
        public async Task<IActionResult> BookFlight(BookDto dto)
        {
            try
            {
                var exisitingUser = await _Context.Users.Where(u => u.Email == dto.email).FirstOrDefaultAsync();
                if (exisitingUser == null) { return NotFound("There no user with such email"); }
                var exisitingFlight = await _Context.Flights.Where(f => f.Id == dto.FlightID).FirstOrDefaultAsync();
                if (exisitingFlight == null) { return NotFound("Something went wrong!"); }
                var existingBooking = await _Context.Bookings.Where(b=>b.userID == exisitingUser.Id && b.FlightID == exisitingFlight.Id).FirstOrDefaultAsync();
                if (existingBooking != null) { return BadRequest("Something went wrong!"); }
                var UserID = exisitingUser.Id;
                var book = new Booking
                {
                    userID = UserID,
                    FlightID = dto.FlightID,
                };
                await _Context.Bookings.AddAsync(book);
                await _Context.SaveChangesAsync();
                return Ok("Booked Successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("MyBookings")]
        public async Task<IActionResult> GetMyBookings(string email)
        {
            try
            {
                var exisitingUser = await _Context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
                if (exisitingUser == null) { return NotFound("There no user with such email"); }

                var myID = exisitingUser.Id;
                var myFlights = await  _Context.Bookings.
                    Where(b => b.userID == myID)
                    .Include(b=>b.User)
                    .Include(b => b.Flight)
                    .Select(b => new
                        {
                           b.Id,
                           b.FlightID,
                           b.Flight.TillDate,
                           b.Flight.FromDate,
                           b.Flight.Description,
                           b.Flight.Name,
                           b.User.Email,
                           b.User.Username,
                        })
                .ToListAsync();
                if (myFlights==null)
                {
                    return NotFound("There no Flights");
                }
                return Ok(myFlights);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
