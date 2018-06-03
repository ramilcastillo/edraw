using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eDraw.api.Controllers.Resources.JobCategoies
{
    public class JobCategoriesResource
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long JobId { get; set; }
        public Core.Models.Jobs Job { get; set; }
    }
}
