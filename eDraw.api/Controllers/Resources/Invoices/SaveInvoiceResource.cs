using System;

namespace eDraw.api.Controllers.Resources.Invoices
{
    public class SaveInvoiceResource
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public string Contractor { get; set; }
        public long JobId { get; set; }
        public decimal Amount { get; set; }
        public long InvoiceTypeId { get; set; }
        public string FilePath { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Status { get; set; }
        public long JobBudgetId { get; set; }
        public string SubContractorId { get; set; }
        public string BankId { get; set; }


    }
}
