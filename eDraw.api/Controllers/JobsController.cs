using AutoMapper;
using eDraw.api.Controllers.Resources.Jobs;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using eDraw.api.ServiceClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eDraw.api.Controllers
{
    [Route("api/[controller]")]
    public class JobsController : Controller
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailServiceClient _emailServiceClient;

        public JobsController(IJobRepository jobRepository,
            IInvoiceRepository repository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IJobBudgetRepository jobBudgetRepostiory,
            IEmailServiceClient emailServiceClient)
        {
            _jobRepository = jobRepository;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailServiceClient = emailServiceClient;
        }

        [Authorize(Roles = "Bank")]
        [HttpPost]
        public async Task<IActionResult> AddNewJob([FromBody] SaveJobResource jobResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (!_emailServiceClient.IsValidEmail(jobResource.GeneralContractorEmail) || !_emailServiceClient.IsValidEmail(jobResource.HomeOwnerEmail))
                {
                    return BadRequest("Invalid Email");
                }

                var prop = new EmailProperties
                {
                    Subject = "Add New Job",
                    Body = "We are testing sending email from AWS",
                    EmailHeading = "This will be heading",
                    HasButton = false,
                    ReceipentsEmail = new List<string>()
                {
                    jobResource.GeneralContractorEmail,
                    jobResource.HomeOwnerEmail
                }
                };

                    var jobs = _mapper.Map<SaveJobResource, Jobs>(jobResource);
                    jobs.UserId = await GetUserId();
                    _jobRepository.SaveJob(jobs);
                await _unitOfWork.CompleteAsync();
      
                return Ok(_mapper.Map<Jobs, JobResource>(jobs));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize(Roles = "Bank,GeneralContractor,HomeOwner")]
        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            try
            {
                var userId = await GetUserId();
                var jobs = await _jobRepository.FindJobsAsync(userId, Request.Query["search"], User.FindFirst(ClaimTypes.Email).Value);
                //var result = _mapper.Map<IEnumerable<Jobs>, IEnumerable<JobResource>>(jobs);
                return Ok(jobs);
            }
            catch (Exception x)
            {
                return BadRequest(x.Message);
            }           
        }

        [Authorize(Roles = "Bank,GeneralContractor,HomeOwner")]
        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJob(long jobId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var job = await _jobRepository.FindJobAsync(jobId);

                if (job == null)
                {
                    return NotFound();
                }

                var jobResource = _mapper.Map<Jobs, JobResource>(job);

                return Ok(jobResource);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Authorize(Roles = "Bank,GeneralContractor,HomeOwner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(long id, [FromBody] SaveJobResource resource)
        {

            var job = await _jobRepository.FindJobPeriodAsync(id);
            if (job == null)
                return NotFound();

            _mapper.Map(resource, job);

            job.UserId = await GetUserId();

            
            await _unitOfWork.CompleteAsync();
            var result = _mapper.Map(job, resource);

            return Ok(result);
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user.Id;
        }
    }
}