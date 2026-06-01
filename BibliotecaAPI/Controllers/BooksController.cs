using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BibliotecaContext _context;
        public BooksController(BibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            return await _context.Books.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            Book book = await _context.Books.FindAsync(id);
            if(book == null) return NotFound();
            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Create(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Book book)
        {
            if (id != book.ID)
            {
                return BadRequest();
            }
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(e => e.ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
    }
}
