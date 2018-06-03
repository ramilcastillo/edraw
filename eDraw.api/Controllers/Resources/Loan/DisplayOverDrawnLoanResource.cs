using System;

namespace eDraw.api.Controllers.Resources.Loan
{
    public class DisplayOverDrawnLoanResource
    {
        public long Id { get; set; }
        public string LoanNo { get; set; }
        public string Name { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime PaidDate { get; set; }
        public decimal ActiveLoan { get; set; }
        public decimal PaidLoan { get; set; }
        public decimal OverDrawnAmount { get; set; }
        public string OverDrawnCategory { get; set; }
        public DateTime MaturityDate { get; set; }
        public decimal ContingencyUsed { get; set; }
        public long BankId { get; set; }
        public long LoanTypeId { get; set; }
        public long InvoiceId { get; set; }
        public string UserId { get; set; }
        public DateTime DateFlaggedAsOverdrawn { get; set; }
    }
}
