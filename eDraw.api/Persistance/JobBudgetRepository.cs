using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eDraw.api.Persistance
{
    public class JobBudgetRepository : IJobBudgetRepository
    {

        private readonly EDrawDbContext _context;

        public JobBudgetRepository(EDrawDbContext context)
        {
            _context = context;
        }

        public void Delete(JobBudgets jobBudget)
        {
            _context.JobBudgets.Remove(jobBudget);
        }

        public void SaveJobBudgets(JobBudgets jobBudgets)
        {
            _context.JobBudgets.Add(jobBudgets);
        }

        public async Task<IEnumerable<JobBudgets>> FindJobBudgetsAsync(long jobBudgetId)
        {
            var result = _context
                .JobBudgets
                .Where(w => w.Id == jobBudgetId)
                .ToListAsync();

            return await result;
        }

        public async Task<IEnumerable<JobBudgets>> FindJobBudgetsByJobIdAsync(long? jobId)
        {
            var result = _context
                .JobBudgets
                .Where(w => w.JobId == jobId)
                .ToListAsync();

            return await result;
        }

        public async Task<JobBudgets> FindSingleJobBudgetsAsync(long jobBudgetId)
        {
            var result = _context
                .JobBudgets
                .SingleOrDefaultAsync(w => w.Id == jobBudgetId);

            return await result;
        }

        public async Task<JobBudgets> GetJobBudgetByIdAsync(long? id)
        {
            return await _context
                .JobBudgets
                    .Include(x => x.Job)
                .SingleOrDefaultAsync(w => w.Id == id);
        }
    }
}
