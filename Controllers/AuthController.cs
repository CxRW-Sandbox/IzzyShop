using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IzzyShop.Data;
using IzzyShop.Models;
using System.Text;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Vulnerable: Weak Password Storage
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Vulnerable: No password hashing
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Registration successful" });
        }

        // Vulnerable: Weak Authentication
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Vulnerable: Plain text password comparison
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
                return Unauthorized();

            // Vulnerable: Weak session token generation
            user.SessionToken = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync();

            return Ok(new { token = user.SessionToken });
        }

        // Vulnerable: Weak Password Reset
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            // Vulnerable: No proper token validation
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return NotFound();

            // Vulnerable: Weak token generation
            user.PasswordResetToken = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync();

            // Vulnerable: Sending reset token in response
            return Ok(new { resetToken = user.PasswordResetToken });
        }

        // Vulnerable: Weak Session Management
        [HttpGet("validate-session")]
        public async Task<IActionResult> ValidateSession(string token)
        {
            // Vulnerable: No token expiration
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.SessionToken == token);

            if (user == null)
                return Unauthorized();

            return Ok(new { username = user.Username, isAdmin = user.IsAdmin });
        }

        // Vulnerable: Weak Password Change
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            // Vulnerable: No password complexity requirements
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return NotFound();

            // Vulnerable: Plain text password storage
            user.Password = request.NewPassword;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully" });
        }

        // Vulnerable: JWT with no signature verification
        [HttpPost("jwt-login")]
        public IActionResult JwtLogin([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
            if (user == null) return Unauthorized();
            var header = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("{\"alg\":\"none\"}"));
            var payload = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{{\"sub\":\"{user.Username}\",\"admin\":{user.IsAdmin.ToString().ToLower()} }}"));
            var token = $"{header}.{payload}."; // No signature
            return Ok(new { token });
        }

        // Vulnerable: Profile with stored XSS
        [HttpPost("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
            if (user == null) return NotFound();
            user.Email = req.Email; // No sanitization
            user.Address = req.Address; // No sanitization
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
    }

    public class ProfileRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
} 