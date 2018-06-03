using System.Collections.Generic;
using System.Threading.Tasks;
using eDraw.api.Controllers.Resources.Invoices;
using eDraw.api.Core.Models;

namespace eDraw.api.Core
{
    public interface IInvoiceRepository
    {
        void Add(Invoices invoice);
        Task<Invoices> GetInvoiceAsync(long? id);
        Task<IEnumerable<Invoices>> FindInvoiceByUserIdAsync(string userId);
        Task<IEnumerable<Invoices>> FindInvoicesByJobIdAsync(long? jobId);
        void Delete(Invoices invoice);
        Task<Invoices> GetInvoiceDisplay(long? invoiceId);
        string GetEmailByUserId(string userId);
        void UpdateInvoiceById(Invoices data);
        Task<IEnumerable<InvoiceTransactionHistory>> FindInvoiceTransactionHistoryByInvoiceId(long? invoiceId);
        Task<IEnumerable<Invoices>> GetInvoiceByJobIdAndStatusAsync(long? jobId, string tabStatus, string role);
    }
}
