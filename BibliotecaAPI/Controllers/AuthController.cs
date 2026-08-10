using BibliotecaAPI.Auth;
using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BibliotecaContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtService _jwtService;

        public AuthController(BibliotecaContext context, PasswordHasher passwordHasher, JwtService jwtService) {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and password are required.");
            }
            if (request.Role != "admin" && request.Role != "bibliotecario")
            {
                return BadRequest("Invalid role");
            }
            if (await _context.UsersSystem.AnyAsync(x => x.Username == request.Username))
            {
                return BadRequest("Username already exists.");
            }
            if (request.Role != "admin" && request.Role != "bibliotecario")
            {
                return BadRequest("Invalid role.");
            }
            var user = new UserSystem
            {
                Username = request.Username,
                PasswordHash = _passwordHasher.Hash(request.Password),
                Role = request.Role
            };
            _context.UsersSystem.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User created");
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _context.UsersSystem.FirstOrDefaultAsync(x => x.Username == request.Usertname);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }
            var result = _jwtService.GenerateToken(user);
            return Ok(new LoginResponse
            {
                Token = result.Token,
                Expiration = result.Expiration
            });
        }
    }
}
