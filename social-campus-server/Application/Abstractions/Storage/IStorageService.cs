using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models.UserModel;

namespace Application.Abstractions.Storage;

public interface IStorageService
{
    Task<string> UploadAsync(Stream fileStream, UserId userId, string contentFor, string contentType,
        CancellationToken cancellationToken = default);

    Task<string?> GetPresignedUrlAsync(string objectKey);

    Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default);

    Task DownloadAsync(string objectKey, Stream destination, CancellationToken cancellationToken = default);
}
