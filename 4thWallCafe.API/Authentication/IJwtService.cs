using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.API.Authentication;

public interface IJwtService
{
    string GenerateToken(Customer user);
    bool ValidatePayload(int customerId, HttpContext httpContext);
}