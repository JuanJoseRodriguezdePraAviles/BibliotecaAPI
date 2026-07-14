using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSystemController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public UserSystemController(BibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<UserSystem>> Get()
        {
            return await _context.UsersSystem.ToListAsync();
        }
    }
}
