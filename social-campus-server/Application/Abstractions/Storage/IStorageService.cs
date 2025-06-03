using Domain.Models.UserModel;

namespace Application.Abstractions.Storage;

public interface IStorageService
{
    Task<string> UploadAsync(Stream fileStream, UserId userId, string contentFor, string contentType,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default);
    Task DownloadAsync(string fileUrl, Stream destination, CancellationToken cancellationToken = default);
}