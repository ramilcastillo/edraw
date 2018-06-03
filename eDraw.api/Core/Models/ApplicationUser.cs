using Microsoft.AspNetCore.Identity;

namespace eDraw.api.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

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

        public string EmailNotification { get; set; }

        public string BankName { get; set; }
    }
}
