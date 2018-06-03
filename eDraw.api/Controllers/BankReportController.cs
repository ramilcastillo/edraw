using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eDraw.api.Controllers.Resources.BankReport;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eDraw.api.Controllers
{
    [Authorize(Roles = "Bank")]
    [Route("api/[controller]")]
    public class BankReportController : Controller
    {
        private readonly IBankReportRepository _repository;
        private readonly IMapper _mapper;

        public BankReportController(IBankReportRepository repository
            , IUnitOfWork unitOfWork
            , IMapper mapper
            , UserManager<ApplicationUser> userManager
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("GetOverDrawnLoan")]
        public async Task<IEnumerable<DisplayOverdrawnLoanResource>> GetOverDrawnLoan()
        {
            var loan = await _repository.GetOverDrawnLoan();
            return _mapper.Map<IEnumerable<DisplayOverdrawnLoanResource>, IEnumerable<DisplayOverdrawnLoanResource>>(loan);
        }

        [HttpGet("GetMaturingLoan")]
        public async Task<IEnumerable<DisplayMaturingLoanResource>> GetMaturingLoan()
        {
            var loan = await _repository.GetMaturingLoan();
            return _mapper.Map<IEnumerable<DisplayMaturingLoanResource>, IEnumerable<DisplayMaturingLoanResource>>(loan);
        }

        [HttpGet("GetStaleLoan")]
        public async Task<IEnumerable<DisplayStaleLoanResource>> GetStaleLoan()
        {
            var loan = await _repository.GetStaleLoan();
            return _mapper.Map<IEnumerable<DisplayStaleLoanResource>, IEnumerable<DisplayStaleLoanResource>>(loan);
        }
    }
}