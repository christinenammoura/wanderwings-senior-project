using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WangerWings.Data;
using WangerWings.Data.Model;

namespace WangerWings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        public LoyaltyController(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        [HttpGet("GetLeaderBoard")]
        public async Task<ActionResult<IEnumerable<leaderboard>>> GetLeaderboard()
        {
            try
            {
                var leaderboard = await _Context.Bookings
                        .Include(b => b.User)
                        .GroupBy(b => new { b.userID, b.User.Email })
                        .Select(group => new leaderboard
                        {
                            UserId = group.Key.userID,
                            Email = group.Key.Email,
                            BookingCount = group.Count()
                        })
                        .OrderByDescending(lb => lb.BookingCount)
                        .ToListAsync();

                return Ok(leaderboard);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
