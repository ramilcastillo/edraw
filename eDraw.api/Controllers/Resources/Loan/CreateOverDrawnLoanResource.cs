using System;

namespace eDraw.api.Controllers.Resources.Loan
{
    public class CreateOverDrawnLoanResource
    {
        public long LoanId { get; set; }
        public string ProjectName { get; set; }
        public string OverdrawnCategory { get; set; }
        public decimal OverdrawnAmount { get; set; }
        public DateTime DateFlagged { get; set; }
    }
}
