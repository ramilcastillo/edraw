using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace eDraw.api.Core.Models
{
    public class Jobs
    {
        public long Id { get; set; }
        public string JobName { get; set; }
        public string Lot { get; set; }
        public string StreetAddress { get; set; }
        public string AptSuite { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string GeneralContractorName { get; set; }
        public string GeneralContractorEmail { get; set; }
        public string HomeOwnerName { get; set; }
        public string HomeOwnerEmail { get; set; }
        public int InterestRate { get; set; }
        public DateTime MaturityDate { get; set; }
        public string Loan { get; set; }
        public decimal LoanAmount { get; set; }
        public string UserId { get; set; }
        public decimal? ContingencyUsed { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<Invoices> Invoices { get; set; }
        public ICollection<JobBudgets> JobBudgets { get; set; }

        public Jobs()
        {
            Invoices = new Collection<Invoices>();
            JobBudgets = new Collection<JobBudgets>();
        }
    }
}
