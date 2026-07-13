using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    public class LendingsController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public LendingsController(BibliotecaContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Lending>> Get() { 
            return await _context.Lendings.ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Lending lending) {
            bool bookExists = await _context.Books.AnyAsync(b => b.ID == lending.BookID);
            bool userExists = await _context.Users.AnyAsync(u => u.ID == lending.UserID);

            if (!bookExists || !userExists) {
                return BadRequest("Book or User not found");
            }
            lending.LendingDate = DateTime.Now;

            _context.Lendings.Add(lending);
            await _context.SaveChangesAsync();

            return Ok(lending);

        }
        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnBook(int id) {
            var lending = await _context.Lendings.FindAsync(id);

            if(lending==null)
            {
                return NotFound();
            }
            lending.ActualReturnDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(lending);
        }
    }
}
