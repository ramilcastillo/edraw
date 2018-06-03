using AutoMapper;
using eDraw.api.Controllers.Resources.Accounts;
using eDraw.api.Controllers.Resources.Bank;
using eDraw.api.Controllers.Resources.BankDashBoard;
using eDraw.api.Controllers.Resources.BankReport;
using eDraw.api.Controllers.Resources.GCDashBoard;
using eDraw.api.Controllers.Resources.HCDashBoard;
using eDraw.api.Controllers.Resources.Invoices;
using eDraw.api.Controllers.Resources.JobBudgets;
using eDraw.api.Controllers.Resources.JobCategoies;
using eDraw.api.Controllers.Resources.Jobs;
using eDraw.api.Controllers.Resources.Profile;
using eDraw.api.Controllers.Resources.Roles;
using eDraw.api.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace eDraw.api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Map DomainModel to API Resource
            #region BankDashBoard
            CreateMap<BankDisplayAllLoanResource, BankDisplayAllLoanResource>();
            CreateMap<BankDisplayPerLoanResource, Loans>();
            CreateMap<BankDisplayLoanApprovedAvailable, Loans>();
            #endregion

            #region HCDashboard

            CreateMap<HomeOwnerDisplayAllActiveInvoicesResource, HomeOwnerDisplayAllActiveInvoicesResource>();
            CreateMap<HomeOwnerDisplayAllLoanResource, HomeOwnerDisplayAllLoanResource>();
            CreateMap<HomeOwnerApprovedVsAvailableResource, HomeOwnerApprovedVsAvailableResource>();
            CreateMap<HomeOwnerDisplayOverdrawnLoanResource, HomeOwnerDisplayOverdrawnLoanResource>();
            CreateMap<HomeOwnerDisplayContingencyUsedResource, HomeOwnerDisplayContingencyUsedResource>();
            #endregion

            #region GeneralContractorDashboard
            CreateMap<GcDisplayAllActiveLoanResource, Loans>();
            CreateMap<GcDisplayAllActiveInvoicesResource, Loans>();
            CreateMap<GcDisplayOverDrawnCategoryResource, Loans>();
            #endregion

            #region Invoice
            CreateMap<DisplayInvoiceResource, Invoices>();
            CreateMap<Loans, BankDisplayPerLoanResource>();
            CreateMap<Invoices, InvoiceResource>()
                 .ForMember(x => x.Job, opt => opt.MapFrom(x => new Jobs
                 {
                     Id = x.Job.Id,
                     JobName = x.Job.JobName,
                     AptSuite = x.Job.AptSuite,
                     City = x.Job.City,
                     ContingencyUsed = x.Job.ContingencyUsed,
                     GeneralContractorEmail = x.Job.GeneralContractorEmail,
                     GeneralContractorName = x.Job.GeneralContractorName,
                     HomeOwnerEmail = x.Job.HomeOwnerEmail,
                     HomeOwnerName = x.Job.HomeOwnerName,
                     InterestRate = x.Job.InterestRate,
                     Loan = x.Job.Loan,
                     LoanAmount = x.Job.LoanAmount,
                     Lot = x.Job.Lot,
                     MaturityDate = x.Job.MaturityDate,
                     State = x.Job.State,
                     StreetAddress = x.Job.StreetAddress,
                     Zip = x.Job.Zip
                 }))
                 .ForMember(x => x.JobBudget, opt => opt.MapFrom(x => new JobBudgets
                 {
                     Id = x.JobBudget.Id,
                     Budget = x.JobBudget.Budget,
                     Category = x.JobBudget.Category,
                     Invoices = x.JobBudget.Invoices,
                     Job = x.JobBudget.Job,
                     JobId = x.JobBudget.JobId,
                     PercentInspected = x.JobBudget.PercentInspected
                 }))
                 .ForMember(x => x.InvoiceType, opt => opt.MapFrom(x => new InvoiceTypes
                 {
                     Id = x.InvoiceType.Id,
                     Invoices = x.InvoiceType.Invoices,
                     Name = x.InvoiceType.Name
                 }))
                 .ForMember(x => x.SubContractor, opt => opt.MapFrom(x => new ApplicationUser
                 {
                    Id = x.SubContractorId
                 }));
            #endregion

            #region BankReportPage

            CreateMap<DisplayOverdrawnLoanResource, DisplayOverdrawnLoanResource>();
            CreateMap<DisplayMaturingLoanResource, DisplayMaturingLoanResource>();
            CreateMap<DisplayStaleLoanResource, DisplayStaleLoanResource>();

            #endregion

            CreateMap<IdentityRole, RoleResource>();

0             CreateMap<Jobs, JobResource>();
                    
            CreateMap<JobBudgets, JobBudgetResource>();

            CreateMap<JobCategories, JobCategoriesResource>();

            CreateMap<Invoices, InvoiceResource>();
            
            #region Profile
            CreateMap<ApplicationUser, HomeOwnerProfileResource>();
            CreateMap<ApplicationUser, BankProfileResource>().ForMember(x => x.BankName, opt => opt.MapFrom(i => i.BusinessName));
            CreateMap<ApplicationUser, SubContractorProfileResource>();
            CreateMap<ApplicationUser, BanksResponse>();
            #endregion

            //Map API Resources to DomainModel
            CreateMap<RegisterResource, ApplicationUser>();
            CreateMap<UpdateProfileRequest, ApplicationUser>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<SaveJobBudgetResource, JobBudgets>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<SaveInvoiceResource, Invoices>()
            .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<SaveJobResource, Jobs>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<JobBudgetResource, JobBudgets>();
            CreateMap<JobCategoriesResource, JobCategories>();

        }
    }
}
