using _4thWallCafe.API.Authentication;
using _4thWallCafe.API.Models;
using _4thWallCafe.API.Utilities;
using _4thWallCafe.App.Services;
using _4thWallCafe.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace _4thWallCafe.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService;
    private readonly ICustomerService _customerService;

    public AuthenticationController(
        ILogger<AuthenticationController> logger, 
        IConfiguration configuration, 
        IJwtService jwtService, 
        ICustomerService customerService)
    {
        _logger = logger;
        _configuration = configuration;
        _jwtService = jwtService;
        _customerService = customerService;
    }
    
    [HttpPost("login")]
    public IActionResult Login(Credentials credentials)
    {
        var customerResult = _customerService.GetCustomerByEmail(credentials.Email);
        if (!customerResult.Ok)
        {
            return Unauthorized("Invalid Email Address");
        }

        var userResult = _customerService.GetCustomerById(customerResult.Data.CustomerID);
        var password = userResult.Data.Password;

        var hashedPw = PasswordEncryption
            .HashPassword(credentials.Password, _configuration["Secret"]);

        if (!hashedPw.SequenceEqual(password))
        {
            return Unauthorized("Invalid Password");
        }

        var token = _jwtService.GenerateToken(userResult.Data);

        return Ok(new { token });
    }
}