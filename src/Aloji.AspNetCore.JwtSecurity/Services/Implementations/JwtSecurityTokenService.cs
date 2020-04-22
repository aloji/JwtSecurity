using Aloji.AspNetCore.JwtSecurity.Context;
using Aloji.AspNetCore.JwtSecurity.Models;
using Aloji.AspNetCore.JwtSecurity.Options;
using Aloji.AspNetCore.JwtSecurity.Services.Contracts;
using Aloji.JwtSecurity.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Threading.Tasks;

namespace Aloji.AspNetCore.JwtSecurity.Services.Implementations
{
    public class JwtSecurityTokenService : IJwtSecurityTokenService
    {
        public virtual async Task<JwtToken> CreateAsync(BaseValidatingContext baseValidatingContext, JwtServerOptions jwtServerOptions)
        {
            Validate();

            var tokenHadler = new TokenHandler(jwtServerOptions);
            var startingDate = DateTime.UtcNow;
            var expiresDate = DateTime.UtcNow.Add(jwtServerOptions.AccessTokenExpireTimeSpan);

            var token = tokenHadler.GenerateToken(
               claims: baseValidatingContext.Claims,
               notBefore: startingDate,
               expires: expiresDate);

            var result = new JwtToken
            {
                AccessToken = token,
                ExpiresIn = this.GetTokenExpiral(startingDate, expiresDate),
                TokenType = JwtBearerDefaults.AuthenticationScheme
            };

            if (jwtServerOptions.RefreshTokenProvider != null)
            {
                var refreshToken = await jwtServerOptions.RefreshTokenProvider.GenerateAsync(baseValidatingContext.Claims);
                if (!string.IsNullOrWhiteSpace(refreshToken))
                    result.RefreshToken = refreshToken;
            }

            void Validate() 
            {
                if (baseValidatingContext == null)
                {
                    throw new ArgumentNullException(nameof(baseValidatingContext));
                }
                if (jwtServerOptions == null)
                {
                    throw new ArgumentNullException(nameof(jwtServerOptions));
                }
            }

            return result;
        }

        protected virtual int GetTokenExpiral(DateTime startingDate, DateTime expiryDate)
           => Convert.ToInt32((expiryDate.ToUniversalTime() - startingDate.ToUniversalTime()).TotalSeconds);
    }
}
