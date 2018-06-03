using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eDraw.api.Controllers.Resources.Loan;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eDraw.api.Controllers
{
    [Route("api/[controller]")]
    public class LoansController : Controller
    {
        private readonly ILoanRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LoansController(ILoanRepository repository, IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPut]
        public async Task<IActionResult> FlaggedOverDrawnLoan()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }        
            _repository.ExtractOverdrawLoan();
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<DisplayOverDrawnLoanResource>> GetFlaggedOverdrawnLoan()
        {
            var result = await _repository.GetFlaggedOverdrawnLoan();
            return _mapper.Map<IEnumerable<Loans>,IEnumerable<DisplayOverDrawnLoanResource>>(result);
        }

    }
}