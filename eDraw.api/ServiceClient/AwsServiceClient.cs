using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using eDraw.api.Controllers;


namespace eDraw.api.ServiceClient
{
    public class AwsServiceClient : IAwsServiceClient
    {
        public async Task<string> UploadAsync(AwsServiceClientSettings awsServiceClientSettings)
        {
            try
            {
                var client = new AmazonS3Client(RegionEndpoint.USEast1);
                var utility = new TransferUtility(client);
                var request = new TransferUtilityUploadRequest
                {
                    BucketName = string.IsNullOrEmpty(awsServiceClientSettings.SubDirectoryInBucket)
                        ? awsServiceClientSettings.BucketName
                        : awsServiceClientSettings.BucketName + @"/" + awsServiceClientSettings.SubDirectoryInBucket,
                    Key = Guid.NewGuid() + Path.GetExtension(awsServiceClientSettings.File.FileName),
                    InputStream = awsServiceClientSettings.File.OpenReadStream(),
                    CannedACL = S3CannedACL.PublicRead
                };

                await utility.UploadAsync(request);

                return $"https://{awsServiceClientSettings.PublicDomain}/{awsServiceClientSettings.SubDirectoryInBucket}/{request.Key}";
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
