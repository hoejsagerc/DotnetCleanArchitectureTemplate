using System.Security.Cryptography;
using System.Text;

namespace Pokemon.Application.Authentication.v1.Common;

public abstract class PasswordHash
{
    public static string Hash(string password)
    {
        var passwordBytes = Encoding.Default.GetBytes(password);

        var hashedPassword = SHA256.HashData(passwordBytes);

        return Convert.ToHexString(hashedPassword);
    }

    public static bool Verify(string password, string hashedPassword)
    {
        var passwordBytes = Encoding.Default.GetBytes(password);
        var hashedInputPassword = SHA256.HashData(passwordBytes);
        var hashedInputPasswordString = Convert.ToHexString(hashedInputPassword);
        return hashedInputPasswordString.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
    }
}