using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eDraw.api.Controllers.Resources.JobCategoies;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace eDraw.api.Controllers
{
    [Route("api/[controller]")]
    public class JobCategoriesController : Controller
    {
        private readonly IJobCategoriesRepository _repository;
        private readonly IMapper _mapper;

        public JobCategoriesController(IJobCategoriesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobCategories(string requestType = null, long? id = null)
        {
            IActionResult result;
            try
            {
                if (requestType != null)
                {
                    switch (requestType)
                    {
                        case "jobcategorybyid":
                            result = Ok(await GetJobCategoriesById(id));
                            break;
                        case "jobcategoriesbyjobid":
                            result = Ok(await GetJobCategoriesByJobId(id));
                            break;
                        default:
                            {
                                result = BadRequest("Please provide the correct request type");
                                break;
                            }
                    }
                }
                else
                {
                    result = BadRequest("Please provide your request type");
                }
            }
            catch (Exception x)
            {

                return BadRequest(x.Message);
            }
            return result;
        }

        private async Task<IActionResult> GetJobCategoriesByJobId(long? id)
        {
            try
            {
                var job = await _repository.GetAllJobCategoriesByJobId(id);
                return Ok(_mapper.Map<IEnumerable<JobCategories>, IEnumerable<JobCategoriesResource>>(job));
            }
            catch
            {
                return BadRequest("Failed to get Job Categories");
            }
        }

        private async Task<IActionResult> GetJobCategoriesById(long? id)
        {
            try
            {
                var job = await _repository.GetJobCategoryById(id);
                return Ok(_mapper.Map<JobCategories, JobCategoriesResource>(job));
            }
            catch
            {
                return BadRequest("Failed to get Job Category");
            }
        }
    }
}