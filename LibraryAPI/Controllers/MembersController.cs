using LibraryAPI.DTOs;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly LibraryContext _context;
        public MembersController(LibraryContext context) => _context = context;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddMember(MemberCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (await _context.Members.AnyAsync(m => m.Email == dto.Email))
                    return BadRequest("Email must be unique.");

                var m = new Member { Name = dto.Name, Email = dto.Email, Phone = dto.Phone };
                _context.Members.Add(m);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMember), new { id = m.MemberId }, m);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to add member", detail = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            try
            {
                var m = await _context.Members.FindAsync(id);
                if (m == null) return NotFound();
                return Ok(m);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve member", detail = ex.Message });
            }
        }

    }
}
