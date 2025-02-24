using Microsoft.AspNetCore.Mvc;
using CodeFirstJWT1.Services;
using CodeFirstJWT1.Models;
using BCrypt.Net;
//using BCrypt.Net;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Security.Crypto.bcrypt;

public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JwtService _jwtService;

    public AuthController(UserService userService, JwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        // Check if username already exists
        if (await _userService.GetUserByUsername(model.Username) != null)
        {
            return BadRequest("Username already exists.");
        }
        // Hash the password for security
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // Create a new user with hashed password
        var user = new User { Username = model.Username, Password = hashedPassword };
        await _userService.AddUser(user);

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userService.GetUserByUsername(model.Username);

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        {
            // ...
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new { Token = token });
    }
}