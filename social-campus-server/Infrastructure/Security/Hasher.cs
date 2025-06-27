using System.Security.Cryptography;
using Application.Abstractions.Security;
using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Security;

public class Hasher(IOptions<HasherOptions> options) : IHasher
{
    private readonly int _saltSize = options.Value.SaltSize;
    private readonly int _hashSize = options.Value.HashSize;
    private readonly int _iterations = options.Value.Iterations;

    private readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

    public async Task<string> HashAsync(string value)
    {
        var salt = RandomNumberGenerator.GetBytes(_saltSize);

        var hash = await Task.Run(() =>
            Rfc2898DeriveBytes.Pbkdf2(value, salt, _iterations, _algorithm, _hashSize));

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public async Task<bool> VerifyAsync(string value, string valueHash)
    {
        var parts = valueHash.Split('-');
        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);

        var inputHash = await Task.Run(() =>
            Rfc2898DeriveBytes.Pbkdf2(value, salt, _iterations, _algorithm, _hashSize));

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}