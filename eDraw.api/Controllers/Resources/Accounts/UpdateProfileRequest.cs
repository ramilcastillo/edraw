using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eDraw.api.Controllers.Resources.Accounts
{
    public class UpdateProfileRequest : IdentityUser
    {
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string BusinessName { get; set; }

        public string TaxId { get; set; }

        public string W9 { get; set; }

        public string RoutingNumber { get; set; }

        public string OriginatingPartyName { get; set; }

        public string AccountNumber { get; set; }

        public string ReceivingPartyName { get; set; }

        public string RoleName { get; set; }

        [Required]
        public string BankName { get; set; }
    }
}
