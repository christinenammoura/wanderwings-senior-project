using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WangerWings.Data;
using WangerWings.Data.Dto;
using WangerWings.Data.Model;

namespace WangerWings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static ConcurrentDictionary<Guid, ChatSession> _sessions = new ConcurrentDictionary<Guid, ChatSession>();
        public ChatbotController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions()
        {
            var start = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\Python312\python.exe",
                Arguments = $"\"C:\\Users\\Dark\\OneDrive\\Desktop\\AI\\ChatBot\\ChatBot.py\" 1 0 1",
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
