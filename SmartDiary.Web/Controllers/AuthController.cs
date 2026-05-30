using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartDiary.Web.Models;
using SmartDiary.Web.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartDiary.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        var user = new User
        {
            UserName = model.Username,
            Email = model.Email
        };

        var result =
            await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        try
        {
            var user =
                await _userManager.FindByNameAsync(model.Username);

            if (user == null)
                return Unauthorized();

            var passwordValid =
                await _userManager.CheckPasswordAsync(
                    user,
                    model.Password);

            if (!passwordValid)
                return Unauthorized();

            var accessToken =
                GenerateJwtToken(user);

            return Ok(new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = Guid.NewGuid().ToString(),
                Username = user.UserName!
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR:");
            Console.WriteLine(ex);
            return BadRequest(ex.Message);
        }
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"];

        Console.WriteLine("KEY = " + jwtKey);
        Console.WriteLine("LENGTH = " + jwtKey?.Length);

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new Exception("JWT KEY IS NULL");
        }

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName!)
    };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}