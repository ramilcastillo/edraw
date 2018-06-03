using System;

namespace eDraw.api.Controllers.Resources.Invoices
{
    public class DisplayInvoiceTransactionHistoryResource
    {
        public long InvoiceId { get; set; }
        public string ActionName { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
