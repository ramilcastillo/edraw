using System;
using eDraw.api.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using eDraw.api.Controllers.Resources.Accounts;
using Microsoft.AspNetCore.Http;
using eDraw.api.Core.Models.AppSettings;
using Microsoft.Extensions.Options;
using eDraw.api.ServiceClient;
using Microsoft.AspNetCore.Authorization;

namespace eDraw.api.Controllers
{
    [Route("api/[controller]")]
    public class ProfilesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AwsAppSettings _awsAppSettings;
        private readonly PhotoAppSettings _photoAppSettings;
        private readonly IAwsServiceClient _awsServiceClient;

        public ProfilesController(UserManager<ApplicationUser> userManager,IMapper mapper, IOptions<PhotoAppSettings> photoSettings, IOptions<AwsAppSettings> awsSettings, IAwsServiceClient awsServiceClient)
        {
            _userManager = userManager;
            _mapper = mapper;
            _photoAppSettings = photoSettings.Value;
            _awsAppSettings = awsSettings.Value;
            _awsServiceClient = awsServiceClient;
        }

        [HttpGet]
        public async Task<ApplicationUser> GetProfiles()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            return await _userManager
                .FindByEmailAsync(email);
        }

         
        [HttpPut]
        [Authorize(Roles ="Bank")]
        public async Task<object> Update(UpdateProfileRequest resource, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var awsServiceclientSettings = new AwsServiceClientSettings(file,
               _awsAppSettings.BucketName, _awsAppSettings.SubFolderProfile, _awsAppSettings.BucketLocation, _awsAppSettings.PublicDomain);
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

                var email = User.FindFirst(ClaimTypes.Email).Value;
                var currentUser = await _userManager.FindByEmailAsync(email);

                currentUser.FirstName = resource.FirstName;
                currentUser.LastName = resource.LastName;
                currentUser.Country = resource.Country;
                currentUser.Address = resource.Address;
                currentUser.State = resource.State;
                currentUser.PhoneNumber = resource.PhoneNumber;
                currentUser.BusinessName = resource.BusinessName;
                currentUser.RoutingNumber = resource.RoutingNumber;
                currentUser.AccountNumber = resource.AccountNumber;
                currentUser.TaxId = resource.TaxId;
                currentUser.OriginatingPartyName = resource.OriginatingPartyName;
                currentUser.ReceivingPartyName = resource.ReceivingPartyName;
                currentUser.BankName = resource.BankName;
                currentUser.W9 = "";

                var result = await _userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    return Ok("Profile is succesfully updated.");
                }
                return BadRequest(result.Errors);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}
