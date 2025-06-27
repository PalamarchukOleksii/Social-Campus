using System.Security.Cryptography;
using System.Text;
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

    public async Task<string?> HashAsync(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        try
        {
            var salt = RandomNumberGenerator.GetBytes(_saltSize);
            var valueBytes = Encoding.UTF8.GetBytes(value);

            var hash = await Task.Run(() =>
                Rfc2898DeriveBytes.Pbkdf2(valueBytes, salt, _iterations, _algorithm, _hashSize));

            var result = $"pbkdf2_sha512${_iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";

            Array.Clear(valueBytes);
            Array.Clear(hash);

            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> VerifyAsync(string value, string valueHash)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(valueHash))
            return false;

        try
        {
            var parts = valueHash.Split('$');
            if (parts is not ["pbkdf2_sha512", _, _, _])
                return false;

            if (!int.TryParse(parts[1], out var iterations) || iterations < 1)
                return false;

            var salt = Convert.FromBase64String(parts[2]);
            var expectedHash = Convert.FromBase64String(parts[3]);
            var valueBytes = Encoding.UTF8.GetBytes(value);

            var actualHash = await Task.Run(() =>
                Rfc2898DeriveBytes.Pbkdf2(valueBytes, salt, iterations, _algorithm, expectedHash.Length));

            var result = CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);

            Array.Clear(valueBytes);
            Array.Clear(actualHash);

            return result;
        }
        catch
        {
            return false;
        }
    }
}