using AutoMapper;
using eDraw.api.Controllers.Resources.SubContractor;
using eDraw.api.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eDraw.api.Controllers
{
    [Authorize(Roles = "Bank, GeneralContractor, HomeOwner")]
    [Route("api/[controller]")]
    public class SubContractorsController : Controller
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IMapper _mapper;

        public SubContractorsController(IRolesRepository rolesRepository, IMapper mapper)
        {
            _rolesRepository = rolesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubContractors()
        {
            var subContractors = await _rolesRepository.GetRoleAsync("SubContractor");
            if (subContractors == null)
                return NotFound("No existing SubContractor");
            return Ok(_mapper.Map<IEnumerable<object>, IEnumerable<SubContractorResponse>>(subContractors));
        }

    }
}