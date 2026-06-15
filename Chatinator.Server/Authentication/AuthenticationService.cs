using System.Security.Cryptography;

namespace Chatinator.Server.Authentication;

public class AuthenticationService
{
    // The sash wringing... the trash thinging... mash flinging... the flash springing, bringing the the crash thinging the... 
    public static (byte[] salt, byte[] hash) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA256, 32);

        return (salt, hash);
    }


    public static bool VerifyPassword(string password, byte[] salt, byte[] hash)
    {
        var testHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(hash, testHash);
    }
}