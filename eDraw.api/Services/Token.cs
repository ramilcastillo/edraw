using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using eDraw.api.Core.Models;
using eDraw.api.Core;

namespace eDraw.api.Services
{
    public class Token : IToken
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private string JwtKey { get; set; }

        public Token(string jwtkey, UserManager<ApplicationUser> userManager) { JwtKey = jwtkey; _userManager = userManager; }

        private bool ValidateToken(string token, out string email, out string role)
        {
            email = role = string.Empty;
            try
            {
                var simplePrinciple = GetPrincipal(token);
                if (!(simplePrinciple?.Identity is ClaimsIdentity identity)) { return false; }
                if (!identity.IsAuthenticated) { return false; }
                var usernameClaim = identity.FindFirst(ClaimTypes.Email);
                email = usernameClaim.Value;
                var usernameroll = identity.FindFirst(ClaimTypes.Role);
                role = usernameroll.Value;
                if (string.IsNullOrEmpty(email)) { return false; }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AuthenticateToken(string token, IConfiguration configuration, out string tokenResult, out string roleResult)
        {
            tokenResult = roleResult = string.Empty;
            if (string.IsNullOrEmpty(token))
            { tokenResult = "No Token Passed"; return false; }

            if (!ValidateToken(token, out var email, out var roll))
            {
                tokenResult = "Invalid Token Or Token Is Not Well Formed ";
                return false;
            }
            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == email);
            if (appUser == null) { tokenResult = "User Not Found."; return false; }
            tokenResult = appUser.Id;
            roleResult = roll;
            return true;
        }


        private ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                if (!(tokenHandler.ReadToken(token) is JwtSecurityToken)) { return null; }
                var symmetricKey = Encoding.UTF8.GetBytes(JwtKey);
                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Token() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
