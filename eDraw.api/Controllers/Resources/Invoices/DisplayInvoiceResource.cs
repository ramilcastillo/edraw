namespace eDraw.api.Controllers.Resources.Invoices
{
    public class DisplayInvoiceResource
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public string Contractor { get; set; }
        public long JobId { get; set; }
        public decimal Amount { get; set; }
        public string FilePath { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public long JobCategoryId { get; set; }
        public long BankId { get; set; }
        public decimal BudgetPrice { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingHandling { get; set; }
        public decimal SalesTax { get; set; }
    }
}
