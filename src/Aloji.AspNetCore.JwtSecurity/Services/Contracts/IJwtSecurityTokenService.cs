using Aloji.AspNetCore.JwtSecurity.Context;
using Aloji.AspNetCore.JwtSecurity.Models;
using Aloji.AspNetCore.JwtSecurity.Options;

namespace Aloji.AspNetCore.JwtSecurity.Services.Contracts
{
    public interface IJwtSecurityTokenService
    {
        JwtToken Create(BaseValidatingContext baseValidatingContext, JwtServerOptions jwtServerOptions);
    }
}
