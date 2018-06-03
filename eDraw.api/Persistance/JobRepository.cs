using eDraw.api.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eDraw.api.Core.Models;

namespace eDraw.api.Persistance
{
    public class JobRepository : IJobRepository
    {
        private readonly EDrawDbContext _context;
        public JobRepository(EDrawDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Jobs>> GetJobsAsync()
        {
            return await _context
                .Jobs
                .Include(x=>x.JobBudgets)
                .ToListAsync();
        }

        public async Task<Jobs> FindJobPeriodAsync(long id)
        {
            return await _context.Jobs.FindAsync(id);
        }
        public async Task<IEnumerable<Invoices>> FindInvoiceJobBudgetAsync(long budgetId)
        {
            var jobBudget = _context.JobBudgets.FirstOrDefault(x => x.Id == budgetId);
            if (jobBudget != null) return await _context.Invoices.Where(x => x.JobId == jobBudget.JobId).ToListAsync();
            return null;
        }

        public async Task<Jobs> GetJobToBuget(long budgetId)
        {
            var buget = await _context.JobBudgets.Where(x => x.Id == budgetId).SingleOrDefaultAsync();
            var job = await _context.Jobs.Where(z => z.Id == buget.JobId).FirstOrDefaultAsync();
            return job;
        }

        public void SaveJob(Jobs job)
        {
            _context.Jobs.Add(job);    
        }

        public async Task<IEnumerable<Jobs>> FindJobsAsync(string userId, string search, string currentUserEmail)
        {
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper();

                return await _context
                    .Jobs
                    .Include(x=>x.JobBudgets)
                    //.Include(x=>x.Invoices)
                    .Where(a => (a.UserId == userId || a.GeneralContractorEmail == currentUserEmail || a.HomeOwnerEmail == currentUserEmail ) && a.JobName.ToUpper().StartsWith(search)).ToListAsync();
            }
            else
                return await _context
                    .Jobs
                    .Include(x=>x.JobBudgets)
                    //.Include(x=>x.Invoices)
                    .Where(a => a.UserId == userId || a.GeneralContractorEmail == currentUserEmail || a.HomeOwnerEmail == currentUserEmail).ToListAsync();
        }
        public async Task<Jobs> FindJobRelatedToBudget(long budgetId)
        {
            return await _context
                .Jobs
                .SingleOrDefaultAsync();
        }

        public async Task<Jobs> FindJobAsync(long jobId)
        {
            return await _context
                .Jobs
                .Include(x=> x.JobBudgets)
                .SingleOrDefaultAsync(w=> w.Id == jobId);
        }
    }
}

