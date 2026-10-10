using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AxiomSysSL.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public AuthController(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public class DevLoginRequest
    {
        public int UserId { get; set; }
        public int CompanyCode { get; set; }
    }

    public class LoginResponse
    {
        public string token { get; set; } = "";
        public DateTime expiresUtc { get; set; }
    }

    [HttpPost("DevLogin")]
    public IActionResult DevLogin(DevLoginRequest request)
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        if (request.UserId <= 0 || request.CompanyCode <= 0)
        {
            return BadRequest();
        }

        var expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiryMinutes"));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var claims = new List<Claim>
        {
            new Claim("Id", request.UserId.ToString()),
            new Claim("CompanyCode", request.CompanyCode.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Expires = expires,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        var token = new JsonWebTokenHandler().CreateToken(tokenDescriptor);

        return Ok(new LoginResponse { token = token, expiresUtc = expires });
    }
}
