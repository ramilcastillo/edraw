using System.Collections.Generic;
using System.Threading.Tasks;
using eDraw.api.Controllers.Resources.Bank;

namespace eDraw.api.Core
{
    public interface IBanksRepository
    {
        Task<IEnumerable<BanksResponse>> GetBanks();
    }
}
