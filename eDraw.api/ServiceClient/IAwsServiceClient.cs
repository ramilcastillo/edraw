using eDraw.api.Controllers;
using System.Threading.Tasks;


namespace eDraw.api.ServiceClient
{
    public interface IAwsServiceClient
    {
        Task<string> UploadAsync(AwsServiceClientSettings awsServiceClientSettings);
    }
}