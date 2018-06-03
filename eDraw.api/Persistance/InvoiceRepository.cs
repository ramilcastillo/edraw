using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using eDraw.api.Controllers.Resources.Invoices;

namespace eDraw.api.Persistance
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly EDrawDbContext _context;

        public InvoiceRepository(EDrawDbContext context)
        {
            _context = context;
        }

        public async Task<Invoices> GetInvoiceDisplay(long? invoiceId)
        {
            var resultList = await _context.Invoices
                .Include(invoice => invoice.InvoiceType)
                .Include(invoice => invoice.Job)
                .Include(invoice => invoice.JobBudget)
                .Where(invoice => invoice.Id == invoiceId)
                .Select(invoice => new Invoices
                {
                    Id = invoice.Id,
                    Amount = invoice.Amount,
                    InvoiceNo = invoice.InvoiceNo,
                    Contractor = invoice.Contractor,
                    FilePath = invoice.FilePath,
                    JobId = invoice.Job.Id,
                }).SingleOrDefaultAsync();

            return resultList;
        }

        public void Add(Invoices invoice)
        {
            _context.Invoices.Add(invoice);
        }

        public IEnumerable<Invoices> FindInvoice(long jobId)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Invoices invoice)
        {
            _context.Remove(invoice);
        }

        public InvoiceStatus GetInvoiceStatus(string userrole, long jobid)
        {
            throw new System.NotImplementedException();
        }

        public void SaveInvoiceStatusAsync(string userrole, long invoiceid, string status)
        {
            throw new System.NotImplementedException();
        }

        public string GetEmailByUserId(string userId)
        {
            var user = _context.UserLogins.SingleOrDefault(s => s.UserId == userId);
            if (user == null)
                return null;
            var result = _context.Users
                .Where(s => s.UserName == user.UserId)
                .Select(s => s.Email).SingleOrDefault();
            return result;
        }

        public async Task<IEnumerable<InvoiceTransactionHistory>> FindInvoiceTransactionHistoryByInvoiceId(long? invoiceId)
        {
            return await _context.InvoiceTransactionHistory.Where(i => i.InvoiceId == invoiceId).ToListAsync();
        }

        public async Task<IEnumerable<Invoices>> GetInvoiceByJobIdAndStatusAsync(long? jobId, string tabStatus, string role)
        {
            IEnumerable<Invoices> invoiceLists = new List<Invoices>();

            if (tabStatus.ToLower().Equals("approved") || tabStatus.ToLower().Equals("rejected"))
            {
                switch (tabStatus.ToLower())
                {
                    case "approved":
                        {
                            invoiceLists = await _context.Invoices
                                .Include(x => x.Bank)
                                .Include(x => x.InvoiceTransactionHistory)
                                .Include(x => x.InvoiceType)
                                .Include(x => x.Job)
                                .Include(x => x.JobBudget)
                                .Include(x => x.Loans)
                                .Include(x => x.SubContractor)
                                .Where(x => x.Status == "Paid").ToArrayAsync();
                            break;
                        }
                    case "rejected":
                        {
                            invoiceLists = await _context.Invoices
                                .Include(x => x.Bank)
                                .Include(x => x.InvoiceTransactionHistory)
                                .Include(x => x.InvoiceType)
                                .Include(x => x.Job)
                                .Include(x => x.JobBudget)
                                .Include(x => x.Loans)
                                .Include(x => x.SubContractor)
                                .Where(x => x.Status == "Rejected").ToArrayAsync();
                            break;
                        }
                    default: break;
                }
            }
            else
            {
                switch (role.ToLower())
                {
                    case "bank":
                        {
                            switch (tabStatus.ToLower())
                            {
                                case "inaction":
                                    {
                                        invoiceLists = await _context.Invoices
                                            .Include(x => x.Bank)
                                            .Include(x => x.InvoiceTransactionHistory)
                                            .Include(x => x.InvoiceType)
                                            .Include(x => x.Job)
                                            .Include(x => x.JobBudget)
                                            .Include(x => x.Loans)
                                            .Include(x => x.SubContractor)
                                            .Where(x => x.Status == "Bank Review").ToArrayAsync();
                                        break;
                                    }

                                default:
                                    {
                                        invoiceLists = new List<Invoices>();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "homeowner":
                        {
                            switch (tabStatus.ToLower())
                            {
                                case "inaction":
                                    {
                                        invoiceLists = await _context.Invoices
                                            .Include(x => x.Bank)
                                            .Include(x => x.InvoiceTransactionHistory)
                                            .Include(x => x.InvoiceType)
                                            .Include(x => x.Job)
                                            .Include(x => x.JobBudget)
                                            .Include(x => x.Loans)
                                            .Include(x => x.SubContractor)
                                            .Where(x => x.Status == "Homeowner Review").ToArrayAsync();
                                        break;
                                    }
                                case "inprogress":
                                    {
                                        invoiceLists = await _context.Invoices
                                            .Include(x => x.Bank)
                                            .Include(x => x.InvoiceTransactionHistory)
                                            .Include(x => x.InvoiceType)
                                            .Include(x => x.Job)
                                            .Include(x => x.JobBudget)
                                            .Include(x => x.Loans)
                                            .Include(x => x.SubContractor)
                                            .Where(x => x.Status == "Bank Review").ToArrayAsync();
                                        break;
                                    }

                                default:
                                    {
                                        invoiceLists = new List<Invoices>();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "generalcontractor":
                        {
                            switch (tabStatus.ToLower())
                            {
                                case "inaction":
                                    {
                                        invoiceLists = await _context.Invoices
                                            .Include(x => x.Bank)
                                            .Include(x => x.InvoiceTransactionHistory)
                                            .Include(x => x.InvoiceType)
                                            .Include(x => x.Job)
                                            .Include(x => x.JobBudget)
                                            .Include(x => x.Loans)
                                            .Include(x => x.SubContractor)
                                            .Where(x => x.Status == "General Contractor Review").ToArrayAsync();
                                        break;
                                    }
                                case "inprogress":
                                    {
                                        invoiceLists = await _context.Invoices
                                            .Include(x => x.Bank)
                                            .Include(x => x.InvoiceTransactionHistory)
                                            .Include(x => x.InvoiceType)
                                            .Include(x => x.Job)
                                            .Include(x => x.JobBudget)
                                            .Include(x => x.Loans)
                                            .Include(x => x.SubContractor)
                                            .Where(x => x.Status == "Bank Review" || x.Status == "Homeowner Review").ToArrayAsync();
                                        break;
                                    }
                                default:
                                    {
                                        invoiceLists = new List<Invoices>();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            return invoiceLists;
        }

        public async Task<Invoices> GetInvoiceAsync(long? id)
        {
            try
            {
                return await _context.Invoices
                        .Include(x => x.SubContractor)
                    .Include(x => x.Job)
                    .Include(x => x.JobBudget)
                    .Include(x => x.InvoiceType)
                    .Include(x => x.Bank)
                    .Where(x => x.Id == id).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void UpdateInvoiceById(Invoices data)
        {
            try
            {
                //_context.Attach(data).State = EntityState.Modified;
                _context.Invoices.Update(data);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public Task<IEnumerable<Invoices>> FindInvoiceByUserIdAsync(string userId)
        {

            //Need to check which user id GC / H / B ???
            throw new System.NotImplementedException();
            //var result = await _context.Invoices.Where(a => a.UserId == userId).ToListAsync();
            //return result;
        }

        public async Task<IEnumerable<Invoices>> FindInvoicesByJobIdAsync(long? jobId)
        {
            try
            {
                var result = await _context
                    .Invoices
                    .Where(a => a.JobId == jobId).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
