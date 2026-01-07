using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestApp.Server.Models;

namespace TestApp.Server.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : Controller
  {
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
      // Todo: check database, etc.
      if (model.Username == "testuser" && model.Password == "password123")
      {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, model.Username),
            new Claim(ClaimTypes.Role, "User")
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
      }
      return Unauthorized(); // 401 Unauthorized if invalid credentials
    }
  }
}