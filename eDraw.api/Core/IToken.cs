using Microsoft.Extensions.Configuration;
using System;

namespace eDraw.api.Core
{
    public interface IToken : IDisposable
    {
        bool AuthenticateToken(string token, IConfiguration configuration, out string tokenResult, out string roleResult);
    }
}
