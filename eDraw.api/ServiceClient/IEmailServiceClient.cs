using System.Threading.Tasks;
using eDraw.api.Core.Models;

namespace eDraw.api.ServiceClient
{
    public interface IEmailServiceClient
    {
        Task<bool> SendEmailAsync(EmailProperties emailProperties);
        bool IsValidEmail(string email);
    }
}
