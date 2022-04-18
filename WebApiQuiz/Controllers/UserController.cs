using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApiQuiz.Models;
using WebApiQuiz.Models.Options;
using WebApiQuiz.Services.Db;

namespace WebApiQuiz.Controllers;

[ApiController]
[Route("Users")]
public class UserController : ControllerBase
{
    private readonly ILogger<QuizController> _logger;

    private readonly DbService _dbService;
    
    public UserController(ILogger<QuizController> logger, DbService dbService)
    {
        _logger = logger;
        _dbService = dbService;
    }
    
    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        if (_dbService.AddUser(user))
        {
            return Ok();
        }
        
        return Conflict("User is already exists");
    }

    [HttpPost("login")]
    public IActionResult LoginUser(User user)
    {
        if (!_dbService.LoginUser(user))
        {
            return Unauthorized("Incorrect email or password");
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)), // время действия 5 минут
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
        var response = new
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(jwt),
            userEmail = user.Email
        };

        return Ok(response);
    }

}