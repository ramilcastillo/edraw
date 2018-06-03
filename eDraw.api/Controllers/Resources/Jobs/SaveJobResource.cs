using System;
using System.Collections.Generic;
using eDraw.api.Controllers.Resources.JobBudgets;

namespace eDraw.api.Controllers.Resources.Jobs
{
    public class SaveJobResource
    {
        public long Id { get; set; }
        public string JobName { get; set; }
        public string Lot { get; set; }
        public string StreetAddress { get; set; }
        public string AptSuite { get; set;}
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string GeneralContractorName { get; set; }
        public string GeneralContractorEmail { get; set; }
        public string HomeOwnerName { get; set; }
        public string HomeOwnerEmail { get; set; }
        public decimal ContingencyUsed { get; set; }
        public int InterestRate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string Loan { get; set; }
        public decimal LoanAmount { get; set; }
        public string UserId { get; set; }
    }
}
