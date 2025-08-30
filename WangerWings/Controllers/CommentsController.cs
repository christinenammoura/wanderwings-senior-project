using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.JSInterop.Infrastructure;
using WangerWings.Data;
using WangerWings.Data.Dto;
using WangerWings.Data.Model;

namespace WangerWings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllComments")]
        public async Task<IActionResult> GetAllCommment()
        {
            try
            {
                var exisitingComments = await _context.Comments
            .Include(c => c.User)
            .Select(c => new
            {
                c.User.Username,
                c.Description

            })
                .ToListAsync();
                if (exisitingComments.Count == 0) { return NotFound("No Comments where found"); }
                return Ok(exisitingComments);

            }
            catch (Exception ex)
            {
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("PostComment")]
        public async Task<IActionResult> Post(CommentDto dto)
        {
            try
            {
                var exisitingUser = await _context.Users.Where(u => u.Email == dto.Email).FirstOrDefaultAsync();
                if (exisitingUser == null)
                {
                    return NotFound("No User with such an email was found");
                }
                var cmnt = new Comment
                {
                    Description = dto.Description,
                    UserID = exisitingUser.Id,
                    CreatedDate = DateTime.Now,
                };

                await _context.Comments.AddAsync(cmnt);
                await _context.SaveChangesAsync();
                return Ok("Added Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
    }
}