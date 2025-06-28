using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Application.Abstractions.Storage;
using Domain.Models.UserModel;
using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly string _bucketName;
    private readonly string _endpoint;
    private readonly IAmazonS3 _s3Client;

    public StorageService(IOptions<StorageOptions> options, IAmazonS3 s3Client)
    {
        _bucketName = options.Value.BucketName;
        _endpoint = options.Value.Endpoint;
        _s3Client = s3Client;

        EnsureBucketExistsAsync().GetAwaiter().GetResult();
    }

    public async Task<string> UploadAsync(Stream fileStream, UserId userId, string contentFor, string contentType,
        CancellationToken cancellationToken = default)
    {
        var extension = contentType == "image/jpeg" ? ".jpg" : ".png";
        var objectKey = $"{contentFor}/{userId.Value}/{Guid.NewGuid()}{extension}";

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            InputStream = fileStream,
            ContentType = contentType,
            AutoCloseStream = true
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);
        var adjustedEndpoint = _endpoint.Contains("/s3")
            ? _endpoint.Replace("/s3", "/object/public")
            : _endpoint;
        return $"{adjustedEndpoint}/{_bucketName}/{objectKey}";
    }

    public async Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        var objectKey = ExtractObjectNameFromUrl(fileUrl);

        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey
        };

        await _s3Client.DeleteObjectAsync(request, cancellationToken);
    }

    public async Task DownloadAsync(string fileUrl, Stream destination, CancellationToken cancellationToken = default)
    {
        var objectKey = ExtractObjectNameFromUrl(fileUrl);

        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey
        };

        using var response = await _s3Client.GetObjectAsync(request, cancellationToken);
        await response.ResponseStream.CopyToAsync(destination, cancellationToken);
    }

    private string ExtractObjectNameFromUrl(string fileUrl)
    {
        var uri = new Uri(fileUrl);
        var path = uri.AbsolutePath.TrimStart('/');
        
        var bucketIndex = path.IndexOf(_bucketName + "/", StringComparison.Ordinal);
        if (bucketIndex < 0)
        {
            return path;
        }
        
        var objectKeyStart = bucketIndex + _bucketName.Length + 1;

        if (objectKeyStart >= path.Length)
            return string.Empty;

        var objectKey = path[objectKeyStart..];
        return Uri.UnescapeDataString(objectKey);
    }


    private async Task EnsureBucketExistsAsync()
    {
        if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName))
            await _s3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = _bucketName
            });
    }
}