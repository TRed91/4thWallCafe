using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace _4thWallCafe.API.Utilities;

public class PasswordEncryption
{
    public static string HashPassword(string password, string secret)
    {
        var bytes = Encoding.UTF8.GetBytes(secret);
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: bytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
    }
}