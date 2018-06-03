using AutoMapper;
using eDraw.api.Controllers.Resources.Bank;
using eDraw.api.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace eDraw.api.Controllers
{
    [Route("api/[controller]")]
    [Authorize()]
    public class BanksController : Controller
    {
        private readonly IRolesRepository _repository;
        private readonly IMapper _mapper;

        public BanksController(IRolesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBanks()
        {
            var banks = await _repository.GetRoleAsync("Bank");

            return Ok(_mapper.Map<IEnumerable<object>, IEnumerable<BanksResponse>>(banks));
        }
    }
}