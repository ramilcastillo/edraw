using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eDraw.api.Controllers.Resources.Invoices
{
    public class InvoiceByStatusResponse
    {
        public long Id { get; set; }

        public string CategoryName { get; set; }

        public decimal TotalBudgeted { get; set; }

        public decimal PaidToDate { get; set; }

        public decimal ApprovedFunds { get; set; }

        public decimal AvailableBudget { get; set; }

        public decimal InvoiceAmount { get; set; }

        public string Status { get; set; }

    }
}
