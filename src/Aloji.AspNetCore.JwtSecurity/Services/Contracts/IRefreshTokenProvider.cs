using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aloji.AspNetCore.JwtSecurity.Services.Contracts
{
    public interface IRefreshTokenProvider
    {
        Task<string> GenerateAsync(IEnumerable<Claim> claims);
    }
}
