using System;

namespace eDraw.api.Controllers.Resources.BankReport
{
    public class DisplayMaturingLoanResource
    {

        public long LoanNumber { get; set; }
        public string ProjectName { get; set; }
        public DateTime DateFlagged { get; set; }
        public decimal PercentComplete { get; set; }
        public DateTime MaturityDate { get; set; }
        public long InvoiceId { get; set; }
    }
}
