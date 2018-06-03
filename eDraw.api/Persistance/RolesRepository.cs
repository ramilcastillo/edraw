using System.Collections.Generic;
using System.Threading.Tasks;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace eDraw.api.Persistance
{
    public class RolesRepository : IRolesRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<object>> GetRoleAsync(string roleName)
        {
            var getSubContractors = await _userManager.GetUsersInRoleAsync(roleName);
            return getSubContractors;
        }
    }
}
