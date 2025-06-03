using Application.Abstractions.Storage;
using Domain.Models.UserModel;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Storage;

public class MinioStorageService : IStorageService
{
    private readonly string _bucketName;
    private readonly IMinioClient _minioClient;

    public MinioStorageService(IConfiguration configuration)
    {
        var minioOptions = configuration.GetSection("Minio");
        _bucketName = minioOptions.GetValue<string>("BucketName")
                      ?? throw new InvalidOperationException("Minio:BucketName configuration is missing.");

        _minioClient = new MinioClient()
            .WithEndpoint(minioOptions.GetValue<string>("Endpoint"))
            .WithCredentials(
                minioOptions.GetValue<string>("AccessKey"),
                minioOptions.GetValue<string>("SecretKey"))
            .WithSSL(minioOptions.GetValue<bool>("UseSSL"))
            .Build();

        EnsureBucketExistsAsync().GetAwaiter().GetResult();
    }

    public async Task<string> UploadAsync(Stream fileStream, UserId userId, string contentFor, string contentType,
        CancellationToken cancellationToken = default)
    {
        var extension = contentType == "image/jpeg" ? ".jpg" : ".png";
        var objectName = $"{contentFor}/{userId}/{Guid.NewGuid()}{extension}";

        var putArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putArgs, cancellationToken);

        return objectName;
    }

    public async Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        var removeArgs = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileUrl);

        await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
    }

    public async Task DownloadAsync(string fileUrl, Stream destination, CancellationToken cancellationToken = default)
    {
        var args = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileUrl)
            .WithCallbackStream(stream => stream.CopyTo(destination));

        await _minioClient.GetObjectAsync(args, cancellationToken);
    }

    private async Task EnsureBucketExistsAsync()
    {
        var found = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucketName));
        if (!found) await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
    }
}