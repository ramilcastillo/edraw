using System.Collections.Generic;

namespace eDraw.api.Controllers.Resources.Invoices
{
    public class DisplayInvoiceStatusResource
    {
        public IEnumerable<InvoiceResource> ActionRequired { get; set; }
        public IEnumerable<InvoiceResource> InProgress { get; set; }
        public IEnumerable<InvoiceResource> Reject { get; set; }
        public IEnumerable<InvoiceResource> Approve { get; set; }
    }
}
