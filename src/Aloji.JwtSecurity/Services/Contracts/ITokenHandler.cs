using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Aloji.JwtSecurity.Services.Contracts
{
    public interface ITokenHandler
    {
        string GenerateToken(IEnumerable<Claim> claims, DateTime? notBefore, DateTime? expires);
        ClaimsPrincipal ValidateToken(string token);
    }
}
