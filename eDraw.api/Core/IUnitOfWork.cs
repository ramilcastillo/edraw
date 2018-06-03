using System.Threading.Tasks;

namespace eDraw.api.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
