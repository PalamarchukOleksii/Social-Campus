using Application.Abstractions.Storage;
using Domain.Models.UserModel;
using Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly string _bucketName;
    private readonly string _endpoint;
    private readonly IMinioClient _minioClient;
    private readonly bool _useSsl;

    public StorageService(IOptions<StorageOptions> options)
    {
        _bucketName = options.Value.BucketName;
        _endpoint = options.Value.Endpoint;
        _useSsl = options.Value.UseSsl;

        _minioClient = new MinioClient()
            .WithEndpoint(_endpoint)
            .WithCredentials(
                options.Value.AccessKey,
                options.Value.SecretKey)
            .WithSSL(_useSsl)
            .Build();
        EnsureBucketExistsAsync().GetAwaiter().GetResult();
    }

    public async Task<string> UploadAsync(Stream fileStream, UserId userId, string contentFor, string contentType,
        CancellationToken cancellationToken = default)
    {
        var extension = contentType == "image/jpeg" ? ".jpg" : ".png";
        var objectName = $"{contentFor}/{userId.Value}/{Guid.NewGuid()}{extension}";

        var putArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putArgs, cancellationToken);

        var protocol = _useSsl ? "https" : "http";
        return $"{protocol}://{_endpoint}/{_bucketName}/{objectName}";
    }

    public async Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        var objectName = ExtractObjectNameFromUrl(fileUrl);

        var removeArgs = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName);

        await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
    }

    public async Task DownloadAsync(string fileUrl, Stream destination, CancellationToken cancellationToken = default)
    {
        var objectName = ExtractObjectNameFromUrl(fileUrl);

        var args = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream => stream.CopyTo(destination));

        await _minioClient.GetObjectAsync(args, cancellationToken);
    }

    private string ExtractObjectNameFromUrl(string fileUrl)
    {
        if (!fileUrl.StartsWith("http://") && !fileUrl.StartsWith("https://")) return fileUrl;
        var uri = new Uri(fileUrl);
        var path = uri.AbsolutePath.TrimStart('/');

        return path.StartsWith($"{_bucketName}/") ? path[(_bucketName.Length + 1)..] : path;
    }

    private async Task EnsureBucketExistsAsync()
    {
        var found = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucketName));
        if (!found) await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
    }
}