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
    [Route("api")]
    public class BorrowController : ControllerBase
    {
        private readonly LibraryContext _context;
        public BorrowController(LibraryContext context) => _context = context;

        [Authorize]
        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow(BorrowDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var member = await _context.Members.FindAsync(dto.MemberId);
                if (member == null)
                    return NotFound(new { message = "Member not found." });

                var book = await _context.Books.FindAsync(dto.BookId);
                if (book == null)
                    return NotFound(new { message = "Book not found." });

                if (book.AvailableCopies <= 0)
                    return BadRequest(new { message = "No copies available." });

                book.AvailableCopies--;

                var record = new BorrowRecord
                {
                    MemberId = dto.MemberId,
                    BookId = dto.BookId,
                    BorrowDate = DateTime.UtcNow,
                    IsReturned = false
                };

                _context.BorrowRecords.Add(record);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Book borrowed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to borrow book", detail = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("return")]
        public async Task<IActionResult> Return(BorrowDto dto)
        {
            try
            {
                var br = await _context.BorrowRecords
                    .FirstOrDefaultAsync(x => x.MemberId == dto.MemberId && x.BookId == dto.BookId && !x.IsReturned);

                if (br == null)
                    return NotFound(new { message = "Borrow record not found." });

                br.IsReturned = true;
                br.ReturnDate = DateTime.UtcNow;

                var book = await _context.Books.FindAsync(dto.BookId);
                if (book != null) book.AvailableCopies++;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Book returned successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to return book", detail = ex.Message });
            }
        }

    }
}
