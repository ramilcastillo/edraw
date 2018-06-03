using System.Collections.Generic;
using System.Threading.Tasks;
using eDraw.api.Core.Models;

namespace eDraw.api.Core
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loans>> GetFlaggedOverdrawnLoan();
        void ExtractOverdrawLoan();
    }
}
