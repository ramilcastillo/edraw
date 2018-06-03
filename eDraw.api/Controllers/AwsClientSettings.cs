using Microsoft.AspNetCore.Http;

namespace eDraw.api.Controllers
{
    public class AwsServiceClientSettings
    {
        public AwsServiceClientSettings(IFormFile file, string bucketName, string subDirectoryInBucket, string bucketLocation, string publicDomain)
        {
            File = file;
            BucketName = bucketName;
            SubDirectoryInBucket = subDirectoryInBucket;
            BucketLocation = bucketLocation;
            PublicDomain = publicDomain;
        }

        public IFormFile File { get; }
        public string BucketName { get; }
        public string SubDirectoryInBucket { get; }
        public string BucketLocation { get; }
        public string PublicDomain { get; }
    }
}
