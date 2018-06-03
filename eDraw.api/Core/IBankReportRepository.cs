using System.Collections.Generic;
using System.Threading.Tasks;
using eDraw.api.Controllers.Resources.BankReport;

namespace eDraw.api.Core
{
    public interface IBankReportRepository
    {
        Task<IEnumerable<DisplayOverdrawnLoanResource>> GetOverDrawnLoan();
        Task<IEnumerable<DisplayMaturingLoanResource>> GetMaturingLoan();
        Task<IEnumerable<DisplayStaleLoanResource>> GetStaleLoan();
    }
}
