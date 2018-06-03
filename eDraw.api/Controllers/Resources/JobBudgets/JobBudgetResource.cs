using eDraw.api.Controllers.Resources.Invoices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using eDraw.api.Controllers.Resources.Jobs;

namespace eDraw.api.Controllers.Resources.JobBudgets
{
    public class JobBudgetResource
    {
        public long Id { get; set; }
        public decimal Budget { get; set; }
        public string Category { get; set; }
        public int? PercentInspected { get; set; }
        public decimal? Paid { get; set; }
        public decimal? CurrentInvoice { get; set; }
        public JobResource Jobs { get; set; }
        public ICollection<InvoiceResource> InvoiceResources { get; set; }

        public JobBudgetResource()
        {
            InvoiceResources = new Collection<InvoiceResource>();
        }
    }
}
