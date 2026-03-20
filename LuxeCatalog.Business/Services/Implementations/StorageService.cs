using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Business.Settings;
using Microsoft.Extensions.Options;

namespace LuxeCatalog.Business.Services.Implementations;

public class StorageService : IStorageService
{
    private readonly CloudflareSettings _settings;
    private readonly AmazonS3Client _s3Client;

    public StorageService(IOptions<CloudflareSettings> settings)
    {
        _settings = settings.Value;

        var credentials = new BasicAWSCredentials(
            _settings.AccessKeyId,
            _settings.SecretAccessKey
        );

        var config = new AmazonS3Config
        {
            ServiceURL = _settings.Endpoint,
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(credentials, config);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder)
    {
        var key = $"{folder}/{fileName}";

        // Convierte el stream a bytes para evitar el error de streaming
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            InputStream = memoryStream,
            ContentType = contentType,
            UseChunkEncoding = false // 👈 soluciona el error de R2
        };

        await _s3Client.PutObjectAsync(request);

        return $"{_settings.PublicUrl}/{key}";
    }

    public async Task<bool> DeleteAsync(string fileUrl)
    {
        try
        {
            var key = fileUrl.Replace($"{_settings.PublicUrl}/", "");

            var request = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request);
            return true;
        }
        catch
        {
            return false;
        }
    }
}