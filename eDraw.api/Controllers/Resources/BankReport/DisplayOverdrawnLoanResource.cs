using System;

namespace eDraw.api.Controllers.Resources.BankReport
{
    public class DisplayOverdrawnLoanResource
    {
        public  long LoanNumber { get; set; }
        public string ProjectName { get; set; }
        public DateTime DateFlagged { get; set; }
        public string OverDrawnCategory { get; set; }
        public decimal OverDrawnAmount { get; set; }
        public long InvoiceId { get; set; }
    }
}
