using System;

namespace eDraw.api.Controllers.Resources.BankDashBoard
{
    public class BankDisplayLoanApprovedAvailable
    { 
        public long Id { get; set; }
        public decimal PercentApproved { get; set; }
        public decimal PercentAvailable { get; set; }
        public DateTime LoanDate { get; set; }
        public long BankId { get; set; }
        public string UserId { get; set; }

    }
}
