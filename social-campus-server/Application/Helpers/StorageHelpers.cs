using System.Security.Cryptography;

namespace Application.Helpers;

public static class StorageHelpers
{
    public static async Task<string> ComputeHashAsync(Stream stream)
    {
        using var sha256 = SHA256.Create();
        var hash = await sha256.ComputeHashAsync(stream);
        return Convert.ToBase64String(hash);
    }
}