using System.Collections.Generic;
using System.Threading.Tasks;
using eDraw.api.Core.Models;

namespace eDraw.api.Core
{
    public interface IJobBudgetRepository
    {
        Task<IEnumerable<JobBudgets>> FindJobBudgetsAsync(long jobId);
        Task<JobBudgets> FindSingleJobBudgetsAsync(long jobId);
        Task<JobBudgets> GetJobBudgetByIdAsync(long? id);
        void Delete(JobBudgets jobBudget);
        void SaveJobBudgets(JobBudgets jobBudgets);
        Task<IEnumerable<JobBudgets>> FindJobBudgetsByJobIdAsync(long? jobId);
    }
}
