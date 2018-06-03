using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eDraw.api.Controllers.Resources.BankDashBoard;
using eDraw.api.Controllers.Resources.HCDashBoard;
using eDraw.api.Controllers.Resources.GCDashBoard;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace eDraw.api.Persistance
{
    public class DashBoardRepository:IDashBoardRepository
    {
        private readonly EDrawDbContext _context;

        public DashBoardRepository(EDrawDbContext context)
        {
            _context = context;
        }

        public async Task<GcDisplayAllActiveLoanResource> GetGcAllActiveLoan()
        {
            var result = new GcDisplayAllActiveLoanResource
            {
                ActiveLoanCount = await _context.Loans.CountAsync(l => l.ActiveLoan > 0)
            };
            return result;
        }

        public async Task<GcDisplayAllActiveInvoicesResource> GetGcAllActiveInvoices()
        {
            var result = new GcDisplayAllActiveInvoicesResource
            {
                ActiveInvoiceCount = await _context.Invoices.Where(s => s.Status == "Active").Select(s => s).CountAsync()
            };
            return result;
        }

        public async Task<GcDisplayOverDrawnCategoryResource> GetGcOverDrawnCategory()
        {
            var result = new GcDisplayOverDrawnCategoryResource
            {
                OverDrawnCategoryNotNullCount = await
                    _context.Loans
                    .Where(s => s.OverDrawnCategory != null || s.OverDrawnCategory != string.Empty)
                        .Select(s => s).CountAsync()
            };
            return result;
        }

        public void CreateLoan(Loans resource)
        {
            var result  = new Loans
            {
                 Id = resource.Id
                , UserId = resource.UserId
                , BankId = resource.BankId
                , Name =  resource.Name
                , ActiveLoan = resource.ActiveLoan
                , LoanDate = resource.LoanDate
                , LoanTypeId = resource.LoanTypeId
                , PaidLoan = resource.PaidLoan
            };
            _context.Loans.Add(result);
            _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BankDisplayPerLoanResource>> BankGetPerLoanTypeByInvoiceId(long? invoiceId)
        {
            List<BankDisplayPerLoanResource> result;
            try
            {
                result = await _context.Loans
                    .Include(loan => loan.LoanType)
                    .Where(loan => loan.InvoiceId == invoiceId)
                    .Select(loan => new BankDisplayPerLoanResource
                    {
                        Id = loan.Id,
                        BankId = loan.Bank.Id,
                        LoanTypeId = loan.LoanType.Id,
                        LoanTypeName = loan.LoanType.Name,
                        Percentage = loan.ActiveLoan / (loan.ActiveLoan + loan.PaidLoan),
                        UserId = loan.UserId
                    }).ToListAsync();
            }
            catch
            {
                return null;
            }

            return result;
        }

        public async Task<IEnumerable<BankDisplayAllLoanResource>> BankGetAllTotalLoanByInvoiceId()
        {
            List<BankDisplayAllLoanResource> result;
            try
            {
                result = await _context.Loans
                    .Include(loan => loan.LoanType)
                    .Select(loan => new BankDisplayAllLoanResource
                    {
                        Id = loan.Id,
                        BankId = loan.Bank.Id,
                        TotalActive = (_context.Loans.Select(s => s.ActiveLoan)).Sum(),
                        TotalPaid = (_context.Loans.Select(s => s.PaidLoan)).Sum(),
                        PaidDate = loan.PaidDate,
                        UserId = loan.UserId
                    }).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public async Task<IEnumerable<BankDisplayLoanApprovedAvailable>> BankGetLoanApprovedAvailable()
        {
            List<BankDisplayLoanApprovedAvailable> result;
            try
            {
                result = await _context.Loans
                    .Include(loan => loan.LoanStatus)
                    .Select(loan => new BankDisplayLoanApprovedAvailable
                    {
                        PercentApproved = (_context.LoanStatus.Select(s => s.PercentageApproved)).Sum(),
                        PercentAvailable = (_context.LoanStatus.Select(s => s.PercentageAvailable)).Sum()
                    }).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }           
            return result;
        }

        public async Task<IEnumerable<HomeOwnerDisplayAllLoanResource>> GetHomeOwnerTotalLoan()
        {
            var loandetail =  _context.Loans
                              .Include(loan=>loan.LoanType)                      
                              .Select(loan=> new HomeOwnerDisplayAllLoanResource
                              {
                                  Id = loan.Id,
                                  BankId = loan.BankId,
                                  TotalActive = (_context.Loans.Select(s => s.ActiveLoan)).Sum(),
                                  TotalPaid = (_context.Loans.Select(s => s.PaidLoan)).Sum(),
                                  PaidDate = loan.PaidDate,
                                  UserId = loan.User.Id
                              }).ToListAsync();
            return await loandetail;
        }

        public async Task<IEnumerable<HomeOwnerApprovedVsAvailableResource>> GetHomeOwnerGetApprovedAvailable()
        {
            var loandetail =  _context.Loans
                              .Include(loan=>loan.LoanStatus)
                              .Select(loan=>new HomeOwnerApprovedVsAvailableResource
                              {
                                  Id = loan.Id,
                                  BankId = loan.BankId,
                                  PercentApproved = (_context.LoanStatus.Select(s => s.PercentageApproved)).Sum(),
                                  PercentAvailable = (_context.LoanStatus.Select(s => s.PercentageAvailable)).Sum(),
                                  LoanDate = loan.LoanDate,
                                  UserId = loan.User.Id
                              }).ToListAsync();
            return await loandetail;
        }

        public async Task<HomeOwnerDisplayAllActiveInvoicesResource> GetHomeOwnerActiveInvoices()
        {
            var loandetail = new HomeOwnerDisplayAllActiveInvoicesResource
            {
                ActiveInvoiceCount = await _context.Invoices.CountAsync(s => s.Status == "Active")
            };
            return loandetail;
        }

        public async Task<HomeOwnerDisplayOverdrawnLoanResource> GetHomeOwnerOverdrawLoans()
        {
            var loandetail = new HomeOwnerDisplayOverdrawnLoanResource
            {
                OverdrawnCount = await _context.Loans.CountAsync(s => s.OverDrawnAmount > 0)
            };
            return loandetail;
        }
        public async Task<HomeOwnerDisplayContingencyUsedResource> GetHomeOwnerContingencyUsedLoans()
        {
            var loandetail = new HomeOwnerDisplayContingencyUsedResource
            {
                PercentContingencyUsed = await _context.Loans.CountAsync(s => s.ContingencyUsed > 0)
            };
            return loandetail;
        }
    }
}