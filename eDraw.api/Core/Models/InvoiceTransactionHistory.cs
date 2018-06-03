using System;

namespace eDraw.api.Core.Models
{
    public class InvoiceTransactionHistory
    {
        public long Id { get; set; }
        public string ActionName { get; set; }
        public DateTime? ActionDate { get; set; }
        public long InvoiceId { get; set; }

        public Invoices Invoice { get; set; }
    }
}
