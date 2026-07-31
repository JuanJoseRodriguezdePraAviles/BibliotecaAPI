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
        private readonly PaswordHasher _passwordHasher;
        private readonly JwtService _jwtService;

        public AuthController(BibliotecaContext context, PasswordHasher passwordHasher, JwtService jwtService) {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
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
                PasswordHash = _passwordHasher.HashPassword(request.Password),
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
            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }
            var token = _jwtService.GenerateToken(user);
            return Ok(new LoginResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(120)
            });
        }
    }
}
