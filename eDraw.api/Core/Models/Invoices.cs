using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace eDraw.api.Core.Models
{
    public class Invoices
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public string Contractor { get; set; }
        public long JobId { get; set; }
        public decimal Amount { get; set; }
        public long InvoiceTypeId { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public long JobBudgetId { get; set; }
        public string SubContractorId { get; set; }
        public string BankId { get; set; }


        [ForeignKey("SubContractorId")]
        public ApplicationUser SubContractor { get; set; }

        [ForeignKey("BankId")]
        public ApplicationUser Bank { get; set; }

        [ForeignKey("InvoiceTypeId")]
        public InvoiceTypes InvoiceType { get; set; }

        [ForeignKey("JobId")]
        public Jobs Job { get; set; }

        [ForeignKey("JobBudgetId")]
        public JobBudgets JobBudget { get; set; }


        public ICollection<InvoiceTransactionHistory> InvoiceTransactionHistory { get; set; }
        public ICollection<Loans> Loans { get; set; }

        public Invoices()
        {
            InvoiceTransactionHistory = new Collection<InvoiceTransactionHistory>();
            Loans = new Collection<Loans>();
        }
    }
}
