using eDraw.api.Core.Models;
using System.Collections.Generic;

namespace eDraw.api.Controllers.Resources.Accounts
{
    public class LoginResourceResponse
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
        public string Token { get; set; }
    }
}
