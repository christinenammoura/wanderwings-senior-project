using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WangerWings.Data;
using WangerWings.Data.Dto;
using WangerWings.Data.Model;
using WangerWings.Services;

namespace WangerWings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        public FlightController(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        [HttpGet("GetAllFlights")]
        public async Task<IActionResult> GetAllFlights()
        {
            try
            {
                var exisitingFlights = await _Context.Flights.ToListAsync();
                if (exisitingFlights == null) { return NotFound("No Flights were found"); }
                return Ok(exisitingFlights);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("CreateFlight")]
        public async Task<string> PostFlight([FromForm] FlightDto dto, [FromForm]PictureDto Pic)
        {
            {
                try
                {
                    Files fileService = new Files();
                    var NewFlight = new Flight
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        FromDate = dto.FromDate,
                        TillDate = dto.TillDate,
                        price = dto.price,
                        FlightPicture = fileService.WriteFile(Pic.Picture)
                    };

                    await _Context.Flights.AddRangeAsync(NewFlight);
                    await _Context.SaveChangesAsync();
                    return "Flight Posted Successfully";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
        [HttpGet("GetFlightPicture")]
        public IActionResult GetFlightPicture(int id)
        {
            try
            {
                var existingFlight = _Context.Flights.Where(u => u.Id == id).FirstOrDefaultAsync();


                var imagePath = Path.Combine("C:\\Users\\Dark\\source\\repos\\WangerWings\\WangerWings\\Upload\\Files", existingFlight.Result.FlightPicture);


                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);


                string contentType = "image/jpeg";


                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions(decimal userMinBudget, decimal userMaxBudget)
        {
            var start = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\Python312\python.exe",
                Arguments = $"\"C:\\Users\\Dark\\OneDrive\\Desktop\\AI\\flight-suggestion.py\" {userMinBudget} {userMaxBudget}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = Process.Start(start))
            {
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit(); 

                if (string.IsNullOrWhiteSpace(result))
                {
                    return NoContent();
                }

                return Ok(result); 
            }


        }
    }
}
