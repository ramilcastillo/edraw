using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eDraw.api.Controllers.Resources.BankDashBoard;
using eDraw.api.Controllers.Resources.HCDashBoard;
using eDraw.api.Controllers.Resources.GCDashBoard;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eDraw.api.Controllers
{
    [Authorize(Roles = "Bank, GeneralContractor, HomeOwner")]
    [Route("api/[controller]")]
    public class DashBoardController : Controller
    {
        private readonly IDashBoardRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashBoardController(IDashBoardRepository repository, IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager; 
        }

        [HttpGet]
        public async Task<IActionResult> GetDashBoard(string role = null, string requestType = null, long? invoiceId = null)
        {
            IActionResult result;
            try
            {
                if (requestType != null && role != null)
                {
                    switch (role.ToLower())
                    {
                        case "generalcontractor":
                            {
                                switch (requestType.ToLower())
                                {
                                    case "overdrawncategory":
                                        {
                                            result = Ok(await GetGcOverDrawnCategory());
                                            break;
                                        }
                                    case "allactiveinvoices":
                                        {
                                            result = Ok(await GetGcAllActiveInvoices());
                                            break;
                                        }
                                    case "allactiveloans":
                                        {
                                            result = Ok(await GetGcAllActiveLoans());
                                            break;
                                        }
                                    default:
                                        {
                                            result = BadRequest("Please provide the correct request type");
                                            break;
                                        }
                                }
                                break;
                            }
                        case "homeowner":
                            {
                                switch (requestType.ToLower())
                                {
                                    case "totalactivepaid":
                                        {
                                            result = Ok(await GetHcTotalActivePaid());
                                            break;
                                        }
                                    case "approvedavailable":
                                        {
                                            result = Ok(await GetHcApprovedAvailable());
                                            break;
                                        }
                                    case "activeinvoices":
                                        {
                                            result = Ok(await GetHcActiveInvoices());
                                            break;
                                        }
                                    case "overdrawnloans":
                                        {
                                            result = Ok(await GetHcOverDrawnLoans());
                                            break;
                                        }
                                    case "contingencyusedloans":
                                        {
                                            result = Ok(await GetHcContingencyUsedLoans());
                                            break;
                                        }
                                    default:
                                        {
                                            result = BadRequest("Please provide the correct request type");
                                            break;
                                        }
                                }
                                break;
                            }
                        case "bank":
                            {
                                switch (requestType.ToLower())
                                {
                                    case "loanpertype":
                                        {
                                            if (invoiceId != null)
                                            {
                                                result = Ok(await GetBankLoanPerType(invoiceId));
                                            }
                                            else
                                            {
                                                result = BadRequest("Please provide the Invoice ID");
                                            }

                                            break;
                                        }
                                    case "totalactivepaid":
                                        {
                                            result = Ok(await GetBankTotalActivePaid());
                                            break;
                                        }
                                    case "approvedavailable":
                                        {
                                            result = Ok(await GetBankApprovedAvailable());
                                            break;
                                        }

                                    default:
                                        {
                                            result = BadRequest("Please provide the correct request type");
                                            break;
                                        }
                                }
                                break;
                            }
                        default:
                            {
                                result = BadRequest();
                                break;
                            }
                    }
                }
                else
                {
                    result = BadRequest("Please provide your request type");
                }
            }
            catch (Exception e)
            {
                result = BadRequest(e.ToString());
            }

            return result;
        }

        private async Task<IActionResult> GetGcOverDrawnCategory()
        {
            var loan = await _repository.GetGcOverDrawnCategory();
            return Ok(_mapper.Map<GcDisplayOverDrawnCategoryResource, GcDisplayOverDrawnCategoryResource>(loan));
        }

        private async Task<IActionResult> GetGcAllActiveInvoices()
        {
            var loan = await _repository.GetGcAllActiveInvoices();
            return Ok(_mapper.Map<GcDisplayAllActiveInvoicesResource, GcDisplayAllActiveInvoicesResource>(loan));
        }

        private async Task<IActionResult> GetGcAllActiveLoans()
        {
            var loan = await _repository.GetGcAllActiveLoan();
            return Ok(_mapper.Map<GcDisplayAllActiveLoanResource, GcDisplayAllActiveLoanResource>(loan));
        }

        private async Task<IActionResult> GetBankLoanPerType(long? invoiceId)
        {
            var loan = await _repository.BankGetPerLoanTypeByInvoiceId(invoiceId);
            return Ok(_mapper.Map<IEnumerable<BankDisplayPerLoanResource>, IEnumerable<BankDisplayPerLoanResource>>(loan));
        }

        private async Task<IActionResult> GetBankTotalActivePaid()
        {
            var loan = await _repository.BankGetAllTotalLoanByInvoiceId();
            return Ok(_mapper.Map<IEnumerable<BankDisplayAllLoanResource>, IEnumerable<BankDisplayAllLoanResource>>(loan));
        }

        private async Task<IActionResult> GetBankApprovedAvailable()
        {
            var loan = await _repository.BankGetLoanApprovedAvailable();
            return Ok(_mapper.Map<IEnumerable<BankDisplayLoanApprovedAvailable>, IEnumerable<BankDisplayLoanApprovedAvailable>>(loan));
        }

        private async Task<IActionResult> GetHcTotalActivePaid()
        {
            var loan = await _repository.GetHomeOwnerTotalLoan();
            return Ok(_mapper.Map<IEnumerable<HomeOwnerDisplayAllLoanResource>, IEnumerable<HomeOwnerDisplayAllLoanResource>>(loan));
        }

        private async Task<IActionResult> GetHcApprovedAvailable()
        {
            var loan = await _repository.GetHomeOwnerGetApprovedAvailable();
            return Ok(_mapper
                .Map<IEnumerable<HomeOwnerApprovedVsAvailableResource>,
                    IEnumerable<HomeOwnerApprovedVsAvailableResource>>(loan));
        }

        private async Task<IActionResult> GetHcActiveInvoices()
        {
            var loan = await _repository.GetHomeOwnerActiveInvoices();
            return Ok(_mapper.Map<HomeOwnerDisplayAllActiveInvoicesResource, HomeOwnerDisplayAllActiveInvoicesResource>(loan));
        }

      
        private async Task<IActionResult> GetHcOverDrawnLoans()
        { 
            var loan = await _repository.GetHomeOwnerOverdrawLoans();
            return Ok(_mapper.Map<HomeOwnerDisplayOverdrawnLoanResource, HomeOwnerDisplayOverdrawnLoanResource>(loan));
        }

        private async Task<IActionResult> GetHcContingencyUsedLoans()
        {
            var loan = await _repository.GetHomeOwnerContingencyUsedLoans();
            return Ok(_mapper.Map<HomeOwnerDisplayContingencyUsedResource, HomeOwnerDisplayContingencyUsedResource>(loan));

        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user.Id;
        }

        private async Task<ApplicationUser> GetUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user;
        }
    }
}