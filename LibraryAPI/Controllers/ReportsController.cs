using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly LibraryContext _context;
        public ReportsController(LibraryContext context) => _context = context;

        [HttpGet("top-borrowed")]
        public async Task<IActionResult> TopBorrowed()
        {
            try
            {
                var top = await _context.BorrowRecords
                    .GroupBy(br => br.BookId)
                    .Select(g => new { BookId = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .Join(_context.Books,
                          g => g.BookId,
                          b => b.BookId,
                          (g, b) => new { b.BookId, b.Title, g.Count })
                    .ToListAsync();

                return Ok(top);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to fetch top borrowed books", detail = ex.Message });
            }
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> Overdue()
        {
            try
            {
                var overdue = await _context.BorrowRecords
                    .Include(br => br.Member)
                    .Include(br => br.Book)
                    .Where(br => !br.IsReturned && br.BorrowDate.AddDays(14) < DateTime.UtcNow)
                    //.Where(br => !br.IsReturned && br.BorrowDate.AddDays(1) < DateTime.UtcNow)
                    .Select(br => new {
                        br.BorrowId,
                        br.BorrowDate,
                        br.Book.Title,
                        br.Member.Name,
                        br.Member.Email
                    }).ToListAsync();

                return Ok(overdue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to fetch overdue books", detail = ex.Message });
            }
        }

    }
}
