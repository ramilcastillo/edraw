using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace eDraw.api.Core.Models
{
    public class JobBudgets
    {


        public long Id { get; set; }
        public decimal Budget { get; set; }
        public long JobId { get; set; }
        public string Category { get; set; }
        public int? PercentInspected { get; set; }
        public decimal? Paid { get; set; }
        public decimal? CurrentInvoice { get; set; }

        public Jobs Job { get; set; }
        public ICollection<Invoices> Invoices { get; set; }

        public JobBudgets()
        {
            Invoices = new Collection<Invoices>();
        }
    }
}
