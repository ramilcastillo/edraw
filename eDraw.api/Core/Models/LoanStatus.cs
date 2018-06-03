using System;

namespace eDraw.api.Core.Models
{
    public class LoanStatus
    {
        public long Id { get; set; }
        public decimal PercentageAvailable { get; set; }
        public decimal PercentageApproved { get; set; }
        public DateTime LoanStatusDate { get; set; }
        public long LoanId { get; set; }

        public Loans Loan { get; set; }
    }
}
