using Aloji.AspNetCore.JwtSecurity.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aloji.AspNetCore.JwtSecurity.Services.Implementations
{
    public class RefreshTokenProvider : IRefreshTokenProvider
    {
        public Func<IEnumerable<Claim>, Task<string>> OnGenerateAsync { get; set; }

        public RefreshTokenProvider()
        {
            this.OnGenerateAsync = context => Task.FromResult<string>(null);
        }

        public async Task<string> GenerateAsync(IEnumerable<Claim> claims)
        {
            return await this.OnGenerateAsync.Invoke(claims);
        }
    }
}
