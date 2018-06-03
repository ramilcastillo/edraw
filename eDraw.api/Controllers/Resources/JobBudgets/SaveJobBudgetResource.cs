namespace eDraw.api.Controllers.Resources.JobBudgets
{
    public class SaveJobBudgetResource
    {
        public long Id { get; set; }
        public decimal Budget { get; set; }
        public long JobId { get; set; }
        public string Category { get; set; }
        public int? PercentInspected { get; set; }
        public decimal? Paid { get; set; }
        public decimal? CurrentInvoice { get; set; }
    }
}
