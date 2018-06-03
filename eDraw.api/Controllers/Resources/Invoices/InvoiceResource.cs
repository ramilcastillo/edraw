using eDraw.api.Controllers.Resources.InvoiceTypes;
using eDraw.api.Controllers.Resources.JobBudgets;
using eDraw.api.Controllers.Resources.Jobs;
using eDraw.api.Core.Models;
using System;

namespace eDraw.api.Controllers.Resources.Invoices
{
    public class InvoiceResource
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public string Contractor { get; set; }
        public decimal Amount { get; set; }
        public long InvoiceTypeId { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public string SubContractorId { get; set; }
        public string BankId { get; set; }
        public string JobBudgetId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public ApplicationUser SubContractor { get; set; }
        public ApplicationUser Bank { get; set; }
        public InvoiceTypeResource InvoiceType { get; set; }
        public JobResource Job { get; set; }
        public JobBudgetResource JobBudget { get; set; }

    }
}
