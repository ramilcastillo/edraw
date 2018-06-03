using System.Collections.Generic;

namespace eDraw.api.Core.Models
{
    public class LoanTypes
    {
        public LoanTypes()
        {
            Loans = new HashSet<Loans>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Loans> Loans { get; set; }
    }
}
