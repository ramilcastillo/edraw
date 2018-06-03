using AutoMapper;
using eDraw.api.Controllers.Resources.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace eDraw.api.Controllers
{
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.ToList();

            if (roles.Count == 0)
                return NotFound();

            var result = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleResource>>(roles);

            return Ok(result);
        }
    }
}
