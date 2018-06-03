using eDraw.api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eDraw.api.Core
{
    public interface IJobCategoriesRepository
    {
        Task<IEnumerable<JobCategories>> GetAllJobCategoriesByJobId(long? jobId);
        Task<JobCategories> GetJobCategoryById(long? id);
    }
}
