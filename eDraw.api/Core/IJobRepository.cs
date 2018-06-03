
using System.Collections.Generic;
using System.Threading.Tasks;
using eDraw.api.Core.Models;

namespace eDraw.api.Core
{
    public interface IJobRepository
    {
        void SaveJob(Jobs job);
        Task<IEnumerable<Jobs>> GetJobsAsync();
        Task<Jobs> FindJobAsync(long jobId);
        Task<IEnumerable<Jobs>> FindJobsAsync(string userId, string search, string currentUserEmail);
        Task<Jobs> FindJobPeriodAsync(long id);
        Task<Jobs> FindJobRelatedToBudget(long budgetId);
    } 
}
