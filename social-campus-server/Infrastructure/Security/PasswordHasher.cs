using Application.Abstractions.Security;
using System.Security.Cryptography;

namespace Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 210_000;

        private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

        public async Task<string> HashAsync(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = await Task.Run(() =>
                Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize));

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public async Task<bool> VerifyAsync(string password, string passwordHash)
        {
            string[] parts = passwordHash.Split('-');
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = await Task.Run(() =>
                Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize));

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
