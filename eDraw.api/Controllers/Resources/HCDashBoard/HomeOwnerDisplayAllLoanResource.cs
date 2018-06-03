using eDraw.api.Core.Models;
using System;

namespace eDraw.api.Controllers.Resources.HCDashBoard
{
    public class HomeOwnerDisplayAllLoanResource
    {
        public long Id { get; set; }
        public decimal TotalActive { get; set; }
        public decimal TotalPaid { get; set; }
        public string BankId { get; set; }
        public string UserId { get; set; }
        public DateTime PaidDate { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser Bank { get; set; }
    }
}
