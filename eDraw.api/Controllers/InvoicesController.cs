using AutoMapper;
using eDraw.api.Controllers.Resources.Invoices;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using eDraw.api.Core.Models.AppSettings;
using eDraw.api.ServiceClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eDraw.api.Controllers
{
    [Authorize(Roles = "Bank,GeneralContractor,HomeOwner")]
    [Route("api/[controller]")]
    public class InvoicesController : Controller
    {
        private readonly AwsAppSettings _awsAppSettings;
        private readonly IInvoiceRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAwsServiceClient _awsServiceClient;
        private readonly PhotoAppSettings _photoAppSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJobBudgetRepository _jobBudgetRepository;

        public InvoicesController(IInvoiceRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IOptions<PhotoAppSettings> photoSettings,
            IOptions<AwsAppSettings> awsSettings,
            IAwsServiceClient awsServiceClient,
            IJobBudgetRepository jobBudgetRepository
            )
        {
            _awsAppSettings = awsSettings.Value;
            _photoAppSettings = photoSettings.Value;
            _repository = repository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jobBudgetRepository = jobBudgetRepository;
            _awsServiceClient = awsServiceClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices(string role = null, string requestType = null, long? invoiceId = null, long? jobId = null, string invoiceStatus = null)
        {
            IActionResult result;

            try
            {
                if (requestType != null)
                {

                    switch (requestType)
                    {
                        case "transactionhistorybyinvoiceid":
                            {
                                result = Ok(await GetTransactionHistoryByInvoiceId(invoiceId));
                                break;
                            }
                        case "invoicedisplaybyinvoiceid":
                            {
                                result = Ok(await GetInvoiceDisplayByInvoiceId(invoiceId));
                                break;
                            }
                        case "invoicebyinvoiceid":
                            {
                                result = Ok(await GetInvoice(invoiceId));
                                break;
                            }
                        case "invoicesbyjobid":
                            {
                                result = Ok(await GetInvoicesByJobId(jobId));
                                break;
                            }
                        case "invoicesbystatus":
                        {
                            if (jobId != null && !string.IsNullOrWhiteSpace(invoiceStatus) &&
                                !string.IsNullOrWhiteSpace(role))
                            {
                                var responseObjList = new List<InvoiceByStatusResponse>();
                                var info = await _repository.GetInvoiceByJobIdAndStatusAsync(jobId, invoiceStatus,
                                    role);
                                foreach (var data in info)
                                {
                                    var response = new InvoiceByStatusResponse();
                                    response.Id = data.Id;
                                    response.CategoryName = data.JobBudget.Category;
                                    response.TotalBudgeted = data.JobBudget.Budget;
                                    response.PaidToDate =  Convert.ToDecimal(data.JobBudget.Paid);
                                    response.ApprovedFunds =
                                        (Convert.ToDecimal(data.JobBudget.PercentInspected) / 100) *
                                        data.JobBudget.Budget;
                                    response.AvailableBudget = response.ApprovedFunds - response.PaidToDate;
                                    response.Status = data.Status;
                                    response.InvoiceAmount = data.Amount;
                                   
                                    responseObjList.Add(response);
                                }
                                result = Ok(responseObjList);
                            }
                            else
                            {
                                result = BadRequest("Please provide all required information get invoices");
                            }
                            
                            break;
                        }
                        default:
                            result = BadRequest("Please provide your request type");
                            break;
                    }
                }
                else
                {
                    result = BadRequest("Please provide your request type");
                }
            }
            catch (Exception x)
            {
                result = BadRequest(x.Message);
            }
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> PostInvoice(SaveInvoiceResource resource, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var awsServiceclientSettings = new AwsServiceClientSettings(file,
                _awsAppSettings.BucketName, _awsAppSettings.SubFolderW9, _awsAppSettings.BucketLocation, _awsAppSettings.PublicDomain);
            var documentUrl = "";
            if (file != null)
            {
                if (file.Length > _photoAppSettings.MaxBytes)
                {
                    return BadRequest("Maximum file size exceeded");
                }
                else
                {
                    if (!_photoAppSettings.IsSupported(file.FileName))
                    {
                        return BadRequest("Invalid file type");
                    }
                    else
                    {
                        documentUrl = await _awsServiceClient.UploadAsync(awsServiceclientSettings);
                    }
                }
            }

            var invoice = _mapper.Map<SaveInvoiceResource, Invoices>(resource);
            invoice.FilePath = documentUrl;

            invoice.BankId = await GetUserId();

            _repository.Add(invoice);
            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map(invoice, resource);
            return Ok(result);
        }

        private async Task<IActionResult> GetTransactionHistoryByInvoiceId(long? id)
        {
            var jobs = await _repository.FindInvoiceTransactionHistoryByInvoiceId(id);
            return Ok(_mapper.Map<IEnumerable<InvoiceTransactionHistory>, IEnumerable<DisplayInvoiceTransactionHistoryResource>>(jobs));
        }

        private async Task<IActionResult> GetInvoiceDisplayByInvoiceId(long? id)
        {
            IActionResult result;
            var invoice = await _repository.GetInvoiceDisplay(id);

            if (invoice != null)
            {
               var response =  new InvoiceResponse
                {
                    Id = invoice.Id,
                    Amount = invoice.Amount,
                    Bank = invoice.Bank,
                    BankId = invoice.BankId,
                    Contractor = invoice.Contractor,
                    FilePath = invoice.FilePath,
                    InvoiceDate = invoice.InvoiceDate,
                    InvoiceNo = invoice.InvoiceNo,
                    InvoiceTypeId = invoice.InvoiceTypeId,
                    InvoiceTypes = invoice.InvoiceType,
                    Job = invoice.Job,
                    JobBudget = invoice.JobBudget,
                    JobBudgetId = invoice.JobBudgetId,
                    JobId = invoice.JobId,
                    SubContractorId = invoice.SubContractorId,
                    Status = invoice.Status,
                    Total = _jobBudgetRepository.FindJobBudgetsAsync(invoice.JobId).Result.Sum(x => x.Budget)
                };

                result = Ok(response);
            }
            else
            {
                return BadRequest("Invoice not existing");
            }
            
            return result;
        }

        private async Task<IActionResult> GetInvoice(long? id)
        {
            var invoice = await _repository.GetInvoiceAsync(id);
            return Ok(invoice);
        }

        private async Task<IActionResult> GetInvoicesByJobId(long? id)
        {
            var invoice = await _repository.FindInvoicesByJobIdAsync(id);
            return Ok(_mapper.Map<IEnumerable<Invoices>, IEnumerable<InvoiceResource>>(invoice));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id,[FromBody] SaveInvoiceResource resource)
        {
            var result = await _repository.GetInvoiceAsync(id);
            if (result == null)
                return NotFound();

            result.InvoiceNo = resource.InvoiceNo;
            result.Contractor = resource.InvoiceNo;
            result.JobId = resource.JobId;
            result.Amount = resource.Amount;
            result.InvoiceTypeId = resource.InvoiceTypeId;
            result.FilePath = resource.FilePath;
            result.InvoiceDate = resource.InvoiceDate;
            result.Status = resource.Status;
            result.JobBudgetId = resource.JobBudgetId;
            result.SubContractorId = resource.SubContractorId;
            result.BankId = resource.BankId;

            _repository.UpdateInvoiceById(result);
            await _unitOfWork.CompleteAsync();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                var invoice = await _repository.GetInvoiceAsync(id);
                if (invoice == null)
                    return NotFound();
                _repository.Delete(invoice);
                await _unitOfWork.CompleteAsync();
                return Ok(id);
            }
            catch (Exception x)
            {
                return BadRequest(x.ToString());
            }
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user.Id;
        }

    }
}