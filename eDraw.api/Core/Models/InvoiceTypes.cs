using System.Collections.Generic;

namespace eDraw.api.Core.Models
{
    public class InvoiceTypes
    {
        public InvoiceTypes()
        {
            Invoices = new HashSet<Invoices>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Invoices> Invoices { get; set; }
    }
}
