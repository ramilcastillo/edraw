using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eDraw.api.Controllers.Resources.BankReport;
using eDraw.api.Core;
using Microsoft.EntityFrameworkCore;

namespace eDraw.api.Persistance
{
    public class BankReportRepository : IBankReportRepository
    {
        private readonly EDrawDbContext _context;

        public BankReportRepository(EDrawDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DisplayOverdrawnLoanResource>> GetOverDrawnLoan()
        {
            var loandetail = (_context.Loans.Where(s => s.OverDrawnAmount > 0)
                .Select(l => new DisplayOverdrawnLoanResource
                {
                    InvoiceId = l.InvoiceId,
                    ProjectName = l.Invoice.Job.JobName,
                    LoanNumber = l.Id,
                    DateFlagged = l.LoanDate,
                    OverDrawnAmount = l.OverDrawnAmount,
                    OverDrawnCategory = l.OverDrawnCategory
                })).ToListAsync();
            return await loandetail;
        }

        public async Task<IEnumerable<DisplayMaturingLoanResource>> GetMaturingLoan()
        {
            var loandetail = (_context.Loans.Where(s=>s.MaturityDate == DateTime.Now)
                .Select(l => new DisplayMaturingLoanResource
                {
                    InvoiceId = l.InvoiceId,
                    ProjectName = l.Invoice.Job.JobName,
                    LoanNumber = l.Id,
                    DateFlagged = l.LoanDate,
                    MaturityDate = l.MaturityDate,
                   PercentComplete = l.PaidLoan / (l.PaidLoan + l.ActiveLoan)                 
                })).ToListAsync();
            return await loandetail;

        }

        public async Task<IEnumerable<DisplayStaleLoanResource>> GetStaleLoan()
        {
            var dateofLastInvoice = _context.Loans
                .OrderByDescending(s => s.LoanDate).Select(s => s.LoanDate).FirstOrDefault();

            var loandetail = (_context.Loans
                .Select(l => new DisplayStaleLoanResource
                {
                    InvoiceId = l.InvoiceId,
                    ProjectName = l.Invoice.Job.JobName,
                    LoanNumber = l.Id,
                    DateFlagged = l.LoanDate,
                    PercentComplete = l.PaidLoan / (l.PaidLoan + l.ActiveLoan),
                    DateOfLastInvoice = dateofLastInvoice                  
                })).ToListAsync();
            return await loandetail;
        }
    }
}
