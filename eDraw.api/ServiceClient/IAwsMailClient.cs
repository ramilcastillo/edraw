using System.Threading.Tasks;

namespace eDraw.api.ServiceClient
{
    public interface IAwsMailClient
    {
        Task<string> SendEmails();
    }
}
