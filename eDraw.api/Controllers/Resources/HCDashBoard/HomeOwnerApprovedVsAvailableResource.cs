using eDraw.api.Core.Models;
using System;

namespace eDraw.api.Controllers.Resources.HCDashBoard
{
    public class HomeOwnerApprovedVsAvailableResource
    {
        public long Id { get; set; }
        public decimal PercentApproved { get; set; }
        public decimal PercentAvailable { get; set; }
        public DateTime LoanDate { get; set; }
        public string BankId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser Bank { get; set; }

    }
}
