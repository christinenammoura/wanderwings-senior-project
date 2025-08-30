using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WangerWings.Data;
using WangerWings.Data.Dto;
using WangerWings.Data.Model;
using WangerWings.Services;

namespace WangerWings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly IConfiguration _configuration;
        public AuthController(ApplicationDbContext Context, IConfiguration configuration)
        {
            _Context = Context;
            _configuration = configuration;
            
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
               
                var user = await _Context.Users.Where(u => u.Email == dto.Email).FirstOrDefaultAsync();
                if (user != null)
                {
                    return BadRequest($"Email{dto.Email} already exists");
                }
                Hashing hashing = new Hashing();
                byte[] passwordHash, passwordSalt;
                hashing.CreatePasswordHash(dto.Password, out passwordHash, out passwordSalt);
                var person = new User
                {
                    Username = dto.Name,
                    Email = dto.Email,  
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    ProfilePicture = "638427584184631939.jpg"
                };

                await _Context.Users.AddAsync(person);
                await _Context.SaveChangesAsync();
                return Ok("Account created successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<string> Login(UserDto dto)
        {
            try
            {
                var user = await _Context.Users.Where(u => u.Email == dto.Email).FirstOrDefaultAsync();
                Hashing hash = new Hashing();
                // if user is not founded 
                if (user == null || !hash.verifyPassword(dto.password, user.PasswordHash, user.PasswordSalt))
                    throw new Exception("incorrect email or password");

                var token = CreateToken(user);
                return token;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private string CreateToken(User client)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , client.Username),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}