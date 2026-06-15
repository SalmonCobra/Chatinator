using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#pragma warning disable CA1416

namespace X509_Certificate_Generator;

internal class Program
{
    private static void Main(string[] args)
    {
        // a function for generating a very complex and long password that is very hard to crack and will probably take longer than the heat death of the universe to do so...
        var length = 128;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                             "abcdefghijklmnopqrstuvwxyz" +
                             "0123456789" +
                             "!@#$%^&*()_+-={}|[];:,./<>?";

        var generatedPassword = "";

        string GeneratePassword()
        {
            return string.Concat(
                Enumerable.Range(0, length)
                    .Select(_ => chars[
                        RandomNumberGenerator.GetInt32(chars.Length)]));
        }

        // generate new password
        generatedPassword = GeneratePassword();

        // enthuse the user about his great success and awesome very long password. Unless he's been stupid and made the password short because he's an idiot and doesn't understand that a password needs to be at least 10^16 combinations, in which case he shall be shamed for this utter display of ignorance.
        var exponent = length * Math.Log10(chars.Length);
        Console.WriteLine("Password Generated Successfully");
        Console.WriteLine($"Unique Characters: {chars.Length}");
        Console.WriteLine($"Password Length: {length}");
        Console.WriteLine($"Possible Combinations: 10^{exponent:F2}");
        Console.WriteLine(exponent >= 16 ? "You're good... lol" : "Bruh pick another password. That's just straight up lazy...");
        Console.WriteLine();


        // Save the generated password to file to later be used as a systemctl variable
        File.WriteAllText("/etc/chatinator.env", $"CHAT_X509_PASSWORD={generatedPassword}");
        Console.WriteLine("Password saved to /etc/chatinator.env");

        // Set the file permissions to 600

        File.SetUnixFileMode("/etc/chatinator.env", UnixFileMode.UserRead | UnixFileMode.UserWrite);

        Console.WriteLine("chatinator.env permissions set to 600");

        // Create key, then generate X509 certificate and export it with generated password.
        using var key = RSA.Create(2048);
        using var cert = new CertificateRequest(new X500DistinguishedName("CN=ChatinatorServer"),
                key,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1)
            .CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(1));

        var pfx = cert.Export(X509ContentType.Pfx, generatedPassword);
        Console.WriteLine("X509 Certificate Exported Successfully");

        // Writing to project directory is temporary until system is ready.
        File.WriteAllBytes("../../../../Chatinator.Server/Data/Certs/server-private.pfx", pfx);
        Console.WriteLine("PFX file saved to temporary development location");

        File.SetUnixFileMode("../../../../Chatinator.Server/Data/Certs/server-private.pfx", UnixFileMode.UserRead | UnixFileMode.UserWrite);
        Console.WriteLine("PFX file permissions set to 600");
    }
}