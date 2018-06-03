using System;

namespace eDraw.api.Controllers.Resources.BankReport
{
    public class DisplayStaleLoanResource
    {
        public long LoanNumber { get; set; }
        public string ProjectName { get; set; }
        public DateTime DateFlagged { get; set; }
        public DateTime DateOfLastInvoice  { get; set; }
        public decimal PercentComplete { get; set; }
        public long InvoiceId { get; set; }
    }
}
