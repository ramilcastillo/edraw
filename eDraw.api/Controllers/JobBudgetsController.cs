using AutoMapper;
using eDraw.api.Controllers.Resources.JobBudgets;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eDraw.api.Controllers
{
    [Route("api/[controller]")]
    public class JobBudgetsController : Controller
    {
        private readonly IJobBudgetRepository _jobBudgetRepostiory;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public JobBudgetsController(IJobBudgetRepository jobBudgetRepostiory, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _jobBudgetRepostiory = jobBudgetRepostiory;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<IActionResult> GetJobBudget(string requestType = null, long? id= null)
        {
            IActionResult result;
            try
            {
                if (requestType != null)
                {
                    switch (requestType)
                    {
                        case "jobbudgetbyid":
                            result = Ok(await GetJobBudgetById(id));
                            break;
                        case "jobbudgetbyjobid":
                            result = Ok(await GetJobBudgetsByJobId(id));
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

        private async Task<IActionResult> GetJobBudgetsByJobId(long? id)
        {
            try
            {
                var job = await _jobBudgetRepostiory.FindJobBudgetsByJobIdAsync(id);
                return Ok(_mapper.Map<IEnumerable<JobBudgets>, IEnumerable<JobBudgetResource>>(job));
            }
            catch
            {
                return BadRequest("Failed to get Job Budgets");
            }
        }

        private async Task<IActionResult> GetJobBudgetById(long? id)
        {
            try
            {
                var job = await _jobBudgetRepostiory.GetJobBudgetByIdAsync(id);
                return Ok(_mapper.Map<JobBudgets, JobBudgetResource>(job));
            }
            catch
            {
                return BadRequest("Failed to get Job Budgets");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaveJobBudgetResource[] resource)
        {
            IActionResult result;
            try
            {
                foreach (var budget in resource)
                {
                    var jobBudget = _mapper.Map<SaveJobBudgetResource, JobBudgets> (budget);
                    _jobBudgetRepostiory.SaveJobBudgets(jobBudget);
                    await _unitOfWork.CompleteAsync();
                }

                result = Ok();
            }
            catch (Exception e)
            {
                result = BadRequest(e.ToString());
            }

            return result;
        }

        // PUT: api/JobBudgets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobBudget(int id, [FromBody]SaveJobBudgetResource jobBudgetResource)
        {
            IActionResult result;

            try
            {
                var jobBudget = await _jobBudgetRepostiory.FindSingleJobBudgetsAsync(id);

                if (jobBudget == null)
                {
                    result = NotFound("No Job Budget Found");
                }
                else
                {
                    var budgetsMap =
                        _mapper.Map<SaveJobBudgetResource, JobBudgets>(jobBudgetResource);
                    _unitOfWork.CompleteAsync();


                    result = Ok(_mapper.Map(jobBudget, jobBudgetResource));
                }
            }
            catch (Exception)
            {

                result = BadRequest("Could not update Job Budget");
            }

            return result;

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var jobBudget = await _jobBudgetRepostiory.FindSingleJobBudgetsAsync(id);
            if (jobBudget != null)
                _jobBudgetRepostiory.Delete(jobBudget);
            else
            {
                return NotFound();
            }
            await _unitOfWork.CompleteAsync();
            return Ok();

        }
    }
}
