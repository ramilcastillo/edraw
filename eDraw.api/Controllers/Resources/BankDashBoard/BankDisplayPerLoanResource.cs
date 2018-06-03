namespace eDraw.api.Controllers.Resources.BankDashBoard
{
    public class BankDisplayPerLoanResource
    {
        public long Id { get; set; }
        public long LoanTypeId { get; set; }
        public string LoanTypeName { get; set; }
        public decimal Percentage { get; set; }
        public string BankId { get; set; }
        public string UserId { get; set; }

    }
}
