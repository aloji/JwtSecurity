using Aloji.AspNetCore.JwtSecurity.Context;
using Aloji.AspNetCore.JwtSecurity.Models;
using Aloji.AspNetCore.JwtSecurity.Options;
using Aloji.AspNetCore.JwtSecurity.Services.Contracts;
using Aloji.JwtSecurity.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace Aloji.AspNetCore.JwtSecurity.Services.Implementations
{
    public class JwtSecurityTokenService : IJwtSecurityTokenService
    {
        public virtual JwtToken Create(BaseValidatingContext baseValidatingContext, JwtServerOptions jwtServerOptions)
        {
            if (baseValidatingContext == null)
            {
                throw new ArgumentNullException(nameof(baseValidatingContext));
            }
            if (jwtServerOptions == null)
            {
                throw new ArgumentNullException(nameof(jwtServerOptions));
            }

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

            return result;
        }

        protected virtual int GetTokenExpiral(DateTime startingDate, DateTime expiryDate)
           => Convert.ToInt32((expiryDate.ToUniversalTime() - startingDate.ToUniversalTime()).TotalSeconds);
    }
}
