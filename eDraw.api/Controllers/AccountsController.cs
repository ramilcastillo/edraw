using Amazon.Runtime.Internal.Util;
using eDraw.api.Controllers.Resources.Accounts;
using eDraw.api.Controllers.Resources.Notification;
using eDraw.api.Core.Models;
using eDraw.api.Core.Models.AppSettings;
using eDraw.api.ServiceClient;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eDraw.api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountsController : Controller
    {
        private readonly AwsAppSettings _awsAppSettings;
        private readonly PhotoAppSettings _photoAppSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IAwsMailClient _mailClient;
        private readonly IAwsServiceClient _awsServiceClient;
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountsController));

        public AccountsController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IConfiguration configuration,IAwsMailClient mailClient,IAwsServiceClient awsServiceClient,IOptions<AwsAppSettings> awsSettings,IOptions<PhotoAppSettings> photoSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mailClient = mailClient;
            _awsServiceClient = awsServiceClient;
            _awsAppSettings = awsSettings.Value;
            _photoAppSettings = photoSettings.Value;
        }

        [HttpPost]
        [ActionName("register")]
        public async Task<object> Register(RegisterResource resource, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                log.Error(BadRequest((ModelState)));
                return BadRequest(ModelState);
            }

            var awsServiceclientSettings = new AwsServiceClientSettings(file,
               _awsAppSettings.BucketName, _awsAppSettings.SubFolderW9, _awsAppSettings.BucketLocation, _awsAppSettings.PublicDomain);
            var documentUrl = "";
            if (file != null)
            {
                if (file.Length > _photoAppSettings.MaxBytes)
                {
                    log.Error("Maximum file size exceeded");
                    return BadRequest("Maximum file size exceeded");
                }
                else
                {
                    if (!_photoAppSettings.IsSupported(file.FileName))
                    {
                        log.Error("Invalid file type");
                        return BadRequest("Invalid file type");
                    }
                    else
                    {
                        documentUrl = await _awsServiceClient.UploadAsync(awsServiceclientSettings);
                        log.Info(documentUrl);
                    }
                }
            }

            var userExist = await _userManager.FindByEmailAsync(resource.Email);
            log.Info(userExist);
            if (userExist != null)
            {
                log.Error("Email already is in use.");
                return BadRequest("Email already is in use.");
            }

            var user = new ApplicationUser
            {
                UserName = resource.Email,
                Email = resource.Email,
                FirstName = resource.FirstName,
                LastName = resource.LastName,
                Country = resource.Country,
                Address = resource.Address,
                State = resource.State,
                PhoneNumber = resource.PhoneNumber,
                BusinessName = resource.BusinessName,
                RoutingNumber = resource.RoutingNumber,
                AccountNumber = resource.AccountNumber,
                TaxId = resource.TaxId,
                OriginatingPartyName = resource.OriginatingPartyName,
                ReceivingPartyName = resource.ReceivingPartyName,
                BankName = resource.BankName,
                W9 = ""
            };

            var result = await _userManager.CreateAsync(user, resource.Password);
            log.Info(result);

            if (result.Succeeded)
            {
                var roleAdd = await _userManager.AddToRoleAsync(user, resource.RoleName);
                log.Info(roleAdd);

                if (roleAdd.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(resource.Email);
                    log.Info(user);

                    return new LoginResourceResponse
                    {
                        User = user,
                        Roles = new[] { resource.RoleName },
                        Token = GenerateJwtToken(user.Email, user)
                    };
                }
                log.Error("Error during adding role to the user.");
                return BadRequest("Error during adding role to the user.");
            }
            log.Error("Error during user creation.");
            return BadRequest("Error during user creation.");
        }

        [HttpPost]
        [ActionName("login")]
        public async Task<object> Login([FromBody] LoginResource login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            log.Info(result);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == login.Email);
                var roles = await _userManager.GetRolesAsync(appUser);
                await _mailClient.SendEmails();          

                return new LoginResourceResponse
                {
                    User = appUser,
                    Roles = roles,
                    Token = GenerateJwtToken(login.Email, appUser),

                };

            }

            log.Error(result);
            return BadRequest(result.ToString());
        }

        [HttpGet]
        [ActionName("current")]
        [Authorize]
        public async Task<object> Current()
        {

            var currentUser = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            log.Info(currentUser);
            var roles = await _userManager.GetRolesAsync(currentUser);
            log.Info(roles);

            return new LoginResourceResponse
            {
                User = currentUser,
                Roles = roles,
            };
        }

        [Authorize(Roles = "Bank")]
        [HttpPost]
        [ActionName("SaveNotificationStatus")]
        public async Task<IActionResult> SaveNotificationStatus([FromBody] NotificationStatusResource statusResource)
        {
            if (statusResource == null) return BadRequest();
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            if (user != null && user.Id != null)
            {
                if (!string.IsNullOrEmpty(statusResource.Email))
                {
                    if (!IsEmail(statusResource.Email)) return BadRequest("Email is not in a valid format");
                    user.EmailNotification = statusResource.Email;
                }
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {                 
                    return Ok("Notification Status Updated");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest("User not found");
        }

        [Authorize(Roles = "Bank")]
        [HttpPut]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatelogInRequest updateRequest)
        {
            if (updateRequest == null) return BadRequest();
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            if (user?.Id != null)
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, updateRequest.CurrentPassword, updateRequest.NewPassword);
                if (passwordResult.Succeeded)
                {

                    return Ok("Password has been updated");
                }
                return BadRequest(passwordResult.Errors);
            }
            return BadRequest("User not found");
        }

        [HttpGet("{email}")]
        [ActionName("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user.Email != null)
            {
                
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var userToken = GenerateJwtToken(user.Email, user, 60);
                
                return BadRequest("Email send failure");
            }
            return BadRequest("User not found");
        }

        [Authorize(Roles = "Bank")]
        [HttpPost]
        [ActionName("ConfirmPassword")]
        public async Task<IActionResult> ConfirmPassword([FromBody] ConfirmPasswordResource passwordResource)
        {
            if (passwordResource == null) return BadRequest();
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            if (user != null && user.Id != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, passwordResource.PasswordResetToken, passwordResource.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(GenerateJwtToken(user.Email, user));
                }

                return BadRequest(result.ToString());
            }
            return BadRequest("Failed to change password");
        }

        private static bool IsEmail(string email)
        {
            try
            {
                return new MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateJwtToken(string email, ApplicationUser user, int expireMinutes = 1000)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };
            var roles = _userManager.GetRolesAsync(user).Result.ToList();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(expireMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtIssuer"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
           return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
