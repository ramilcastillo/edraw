using eDraw.api.Core.Models;
using System.Collections.Generic;

namespace eDraw.api.Controllers.Resources.Accounts
{
    public class GetProfileResource : ApplicationUser
    {
        public List<string> Roles { get; set; }
    }
}
