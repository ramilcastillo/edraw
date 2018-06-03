using System.Collections.Generic;
using System.Threading.Tasks;
using eDraw.api.Controllers.Resources.BankDashBoard;
using eDraw.api.Controllers.Resources.HCDashBoard;
using eDraw.api.Controllers.Resources.GCDashBoard;
using eDraw.api.Core.Models;

namespace eDraw.api.Core
{
    public interface IDashBoardRepository
    {
        void CreateLoan(Loans loan);
        Task<IEnumerable<BankDisplayPerLoanResource>> BankGetPerLoanTypeByInvoiceId(long? invoiceId);
        Task<IEnumerable<BankDisplayAllLoanResource>> BankGetAllTotalLoanByInvoiceId();
        Task<IEnumerable<BankDisplayLoanApprovedAvailable>> BankGetLoanApprovedAvailable();
        Task<IEnumerable<HomeOwnerApprovedVsAvailableResource>> GetHomeOwnerGetApprovedAvailable();
        Task<HomeOwnerDisplayAllActiveInvoicesResource> GetHomeOwnerActiveInvoices();
        Task<IEnumerable<HomeOwnerDisplayAllLoanResource>> GetHomeOwnerTotalLoan();
        Task<HomeOwnerDisplayOverdrawnLoanResource> GetHomeOwnerOverdrawLoans();
        Task<HomeOwnerDisplayContingencyUsedResource> GetHomeOwnerContingencyUsedLoans();
        Task<GcDisplayAllActiveLoanResource> GetGcAllActiveLoan();
        Task<GcDisplayAllActiveInvoicesResource> GetGcAllActiveInvoices();
        Task<GcDisplayOverDrawnCategoryResource> GetGcOverDrawnCategory();
    }

}
