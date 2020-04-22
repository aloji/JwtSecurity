using Aloji.AspNetCore.JwtSecurity.Context;
using Aloji.AspNetCore.JwtSecurity.Models;
using Aloji.AspNetCore.JwtSecurity.Options;
using System.Threading.Tasks;

namespace Aloji.AspNetCore.JwtSecurity.Services.Contracts
{
    public interface IJwtSecurityTokenService
    {
        Task<JwtToken> CreateAsync(BaseValidatingContext baseValidatingContext, JwtServerOptions jwtServerOptions);
    }
}
