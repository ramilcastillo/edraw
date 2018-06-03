using System;

namespace eDraw.api.Controllers.Resources.BankDashBoard
{
    public class BankCreateLoanResource
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LoanDate { get; set; }
        public decimal ActiveLoan { get; set; }
        public decimal PaidLoan { get; set; }
        public int BankId { get; set; }
        public int LoanTypeId { get; set; }

    }
}
