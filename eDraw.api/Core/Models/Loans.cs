using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eDraw.api.Core.Models
{
    public class Loans
    {
        public Loans()
        {
            LoanStatus = new HashSet<LoanStatus>();
        }

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
        public string BankId { get; set; }
        public long LoanTypeId { get; set; }
        public long InvoiceId { get; set; }
        public string UserId { get; set; }
        public DateTime DateFlaggedAsOverdrawn { get; set; }

        [ForeignKey("BankId")]
        public ApplicationUser Bank { get; set; }
        public Invoices Invoice { get; set; }
        public LoanTypes LoanType { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<LoanStatus> LoanStatus { get; set; }
    }
}
