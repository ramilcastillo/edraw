using System.Collections.Generic;
using System.Threading.Tasks;

namespace eDraw.api.Core
{
    public interface IRolesRepository
    {
        Task<IEnumerable<object>> GetRoleAsync(string roleName);
    }
}
