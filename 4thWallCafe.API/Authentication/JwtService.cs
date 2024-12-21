using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using _4thWallCafe.Core.Entities;
using Microsoft.IdentityModel.Tokens;

namespace _4thWallCafe.API.Authentication;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <summary>
    /// Generates a Json Web Token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string GenerateToken(Customer user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Secret"]));
        var credentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        
        // create payload
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Country, user.Country),
            new Claim("CustomerID", user.CustomerID.ToString())
        };
        
        // Create token
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:Expiration")),
            signingCredentials: credentials,
            claims: claims
            );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Checks if the id of the requested customer data is the same stored in the token.
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="httpContext"></param>
    /// <returns>true if the ids match, otherwise false</returns>
    public bool ValidatePayload(int customerId, HttpContext httpContext)
    {
        // extract id from user claims.
        var payload = httpContext.User.Claims
            .FirstOrDefault(c => c.Type == "CustomerID")?.Value;
        if (payload == null)
        {
            return false;
        }
        return int.Parse(payload) == customerId;
    }
}