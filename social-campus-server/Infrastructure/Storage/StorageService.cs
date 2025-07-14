using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Application.Abstractions.Storage;
using Domain.Models.UserModel;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Storage
{
    public class StorageService : IStorageService
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _s3Client;

        public StorageService(IOptions<StorageOptions> options, IAmazonS3 s3Client)
        {
            _bucketName = options.Value.BucketName;
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

            return objectKey;
        }

        public async Task<string?> GetPresignedUrlAsync(string objectKey, TimeSpan? expiry = null)
        {
            if (string.IsNullOrWhiteSpace(objectKey))
                return null;
            
            if (!await DoesObjectExistAsync(objectKey))
                return null;

            expiry ??= TimeSpan.FromMinutes(15);

            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                Expires = DateTime.UtcNow.Add(expiry.Value),
                Verb = HttpVerb.GET,
                Protocol = Protocol.HTTP
            };

            return await _s3Client.GetPreSignedURLAsync(request);
        }

        public async Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey
            };

            await _s3Client.DeleteObjectAsync(request, cancellationToken);
        }

        public async Task DownloadAsync(string objectKey, Stream destination, CancellationToken cancellationToken = default)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey
            };

            using var response = await _s3Client.GetObjectAsync(request, cancellationToken);
            await response.ResponseStream.CopyToAsync(destination, cancellationToken);
        }

        private async Task<bool> DoesObjectExistAsync(string objectKey, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = objectKey
                };

                await _s3Client.GetObjectMetadataAsync(request, cancellationToken);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        private async Task EnsureBucketExistsAsync()
        {
            if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName))
            {
                await _s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = _bucketName
                });
            }
        }
    }
}
