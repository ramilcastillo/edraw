using eDraw.api.Core.Models;
using System;

namespace eDraw.api.Controllers.Resources.Invoices
{
    public class InvoiceResponse
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public string Contractor { get; set; }
        public string SubContractorId { get; set; }
        public long JobId { get; set; }
        public decimal Amount { get; set; }
        public long InvoiceTypeId { get; set; }
        public string FilePath { get; set; }
        public string UserId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string BankId { get; set; }
        public long? JobBudgetId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public ApplicationUser Bank { get; set; }
        public Core.Models.InvoiceTypes InvoiceTypes { get; set; }

        public Core.Models.Jobs Job { get; set; }

        public Core.Models.JobBudgets JobBudget { get; set; }

        public ApplicationUser User { get; set; }
    }
}
