using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eDraw.api.Persistance
{
    public class JobCategoriesRepository : IJobCategoriesRepository
    {
        private readonly EDrawDbContext _context;

        public JobCategoriesRepository(EDrawDbContext context)
        {
            _context = context;
        }

        public async Task<JobCategories> GetJobCategoryById(long? id)
        {
            return await _context.JobCategories
                            .Where(jc => jc.Id == id)
                            .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<JobCategories>> GetAllJobCategoriesByJobId(long? jobId)
        {
            return await _context.JobCategories
                            .Where(jc => jc.JobId == jobId)
                            .ToListAsync();
        }
    }
}
