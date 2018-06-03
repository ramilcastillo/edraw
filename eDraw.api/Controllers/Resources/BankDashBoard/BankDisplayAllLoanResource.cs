using System;

namespace eDraw.api.Controllers.Resources.BankDashBoard
{
    public class BankDisplayAllLoanResource
    {
        public long Id { get; set; }
        public decimal TotalActive { get; set; }
        public decimal TotalPaid { get; set; }
        public string BankId { get; set; }
        public string UserId { get; set; }
        public DateTime PaidDate { get; set; }

    }
}
