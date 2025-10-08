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
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;
        public BooksController(LibraryContext context) => _context = context;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBook(BookCreateDto dto)
        {
            try
            {
                if (await _context.Books.AnyAsync(b => b.Isbn == dto.Isbn))
                    return BadRequest(new { message = "Book with this ISBN already exists." });

                var book = new Book
                {
                    Title = dto.Title,
                    Author = dto.Author,
                    Isbn = dto.Isbn,  // match exact property
                    PublishedYear = dto.PublishedYear,
                    AvailableCopies = dto.AvailableCopies
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBooks), new { id = book.BookId }, book);
            }
            catch (Exception ex)
            {
                // Return proper error with 500 status code
                return StatusCode(500, new { message = "Failed to add book", detail = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var data = await _context.Books
                    .Select(b => new
                    {
                        b.BookId,
                        b.Title,
                        b.Author,
                        b.Isbn,
                        b.PublishedYear,
                        b.AvailableCopies,
                        IsAvailable = b.AvailableCopies > 0
                    }).ToListAsync();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to fetch books", detail = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var b = await _context.Books.FindAsync(id);
                if (b == null) return NotFound(new { message = "Book not found" });

                return Ok(b);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to fetch book", detail = ex.Message });
            }
        }

    }
}