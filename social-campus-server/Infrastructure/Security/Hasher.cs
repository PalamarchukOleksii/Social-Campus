using System.Security.Cryptography;
using Application.Abstractions.Security;

namespace Infrastructure.Security;

public class Hasher : IHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 210_000;

    private readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

    public async Task<string> HashAsync(string value)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var hash = await Task.Run(() =>
            Rfc2898DeriveBytes.Pbkdf2(value, salt, Iterations, _algorithm, HashSize));

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public async Task<bool> VerifyAsync(string value, string valueHash)
    {
        var parts = valueHash.Split('-');
        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);

        var inputHash = await Task.Run(() =>
            Rfc2898DeriveBytes.Pbkdf2(value, salt, Iterations, _algorithm, HashSize));

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}