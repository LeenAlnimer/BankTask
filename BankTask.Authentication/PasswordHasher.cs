using System.Security.Cryptography;
using BankTask.Application.Interfaces.Security;

namespace BankTask.Authentication;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;

    private const int CurrentIterations = 100_000;
    private const int LegacyIterations = 100_000;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            CurrentIterations,
            HashAlgorithmName.SHA256,
            HashSize);

        return $"{CurrentIterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split(':');

        int iterations;
        string saltPart;
        string hashPart;

        if (parts.Length == 3)
        {
            iterations = int.Parse(parts[0]);
            saltPart = parts[1];
            hashPart = parts[2];
        }
        else if (parts.Length == 2)
        {
            iterations = LegacyIterations;
            saltPart = parts[0];
            hashPart = parts[1];
        }
        else
        {
            return false;
        }

        var salt = Convert.FromBase64String(saltPart);
        var storedHash = Convert.FromBase64String(hashPart);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        return CryptographicOperations.FixedTimeEquals(
            hash,
            storedHash);
    }
}