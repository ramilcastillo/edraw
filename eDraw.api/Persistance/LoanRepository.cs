using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace eDraw.api.Persistance
{
    public class LoanRepository:ILoanRepository
    {
        private readonly EDrawDbContext _context;

        public LoanRepository(EDrawDbContext context)
        {
            _context = context;
        }

        public void ExtractOverdrawLoan()
        {
            try
            {
                var jobAmountBudgetByCategory = _context.JobBudgets
                    .Select(budget => new
                    {
                        budget.Job.LoanAmount,
                        budget.Category,
                        budget.JobId
                    })
                    .GroupBy(budget => budget.Category, budget => budget.LoanAmount,
                        (key, values) => new {Category = key, LoanAmount = values.Sum()})
                    .ToList();

                var jobCategory = _context.JobBudgets.Join(jobAmountBudgetByCategory.AsEnumerable(), j => j.Category,
                    jc => jc.Category, (j, jc) => new
                    {
                        JBudget = j.Budget,
                        JCategory = jc.Category,
                        SumAmount = jc.LoanAmount
                    }).Distinct();

                var jobCategoryFiltered = jobCategory.Where(category => category.SumAmount > category.JBudget).ToList();

                var invoicesByCategory = _context.Invoices
                    .Include(invoice => invoice.JobBudget)
                    .Join(jobCategoryFiltered.AsEnumerable(), i => i.JobBudget.Category, jcf => jcf.JCategory,
                        (i, jcf) => new
                        {
                            i.Id,
                            Overdrawn = jcf.SumAmount - jcf.JBudget,
                            Category = jcf.JCategory
                        }).ToList();

                var overDrawnLoans = _context.Loans
                    .Include(e=>e.Invoice)
                    .Where(loan=> invoicesByCategory.Any(r=>r.Id == loan.InvoiceId))
                    .Join(invoicesByCategory, l => l.InvoiceId, ic => ic.Id, (l, ic) => new Loans
                    {
                        Id = l.Id,
                        ActiveLoan = l.ActiveLoan,
                        BankId = l.BankId,
                        ContingencyUsed = l.ContingencyUsed,
                        DateFlaggedAsOverdrawn = l.DateFlaggedAsOverdrawn,
                        InvoiceId = ic.Id,
                        LoanDate = l.LoanDate,
                        LoanNo = l.LoanNo,
                        LoanTypeId = l.LoanTypeId,
                        MaturityDate = l.MaturityDate,
                        Name = l.Name,
                        OverDrawnAmount = ic.Overdrawn,
                        OverDrawnCategory =ic.Category,
                        PaidDate = l.PaidDate,
                        PaidLoan = l.PaidLoan,
                        UserId = l.UserId
                    }).Distinct().ToList();

                foreach (var s in overDrawnLoans)
                {
                    _context.Loans.Update(s);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        public async Task<IEnumerable<Loans>> GetFlaggedOverdrawnLoan()
        {
            var result = await _context.Loans
                .Where(l => l.OverDrawnAmount > 0)
                .Select(l => l)
                .ToListAsync();
            return  result;
        }
    }
}
