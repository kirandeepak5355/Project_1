using LibraryAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _key = "ThisIsASuperSecretKeyForJWT_1234567890";

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(UserLoginDto dto)
        {
            try
            {
                if (dto.Username != "admin" || dto.Password != "password")
                    return Unauthorized("Invalid credentials");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_key);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, dto.Username)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    Issuer = "LibraryApi",
                    Audience = "LibraryApiUser",
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { Token = tokenHandler.WriteToken(token) });
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
