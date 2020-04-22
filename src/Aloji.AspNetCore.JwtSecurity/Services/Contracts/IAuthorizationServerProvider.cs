using Aloji.AspNetCore.JwtSecurity.Context;
using System.Threading.Tasks;

namespace Aloji.AspNetCore.JwtSecurity.Services.Contracts
{
    public interface IAuthorizationServerProvider
    {
        Task GrantClientCredentialsAsync(GrantResourceOwnerCredentialsContext context);
        Task GrantRefreshTokenAsync(GrantRefreshTokenContext context);
    }
}
