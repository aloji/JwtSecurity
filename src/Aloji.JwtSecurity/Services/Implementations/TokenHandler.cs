using Aloji.JwtSecurity.Options;
using Aloji.JwtSecurity.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aloji.JwtSecurity.Services.Implementations
{
    public class TokenHandler : ITokenHandler
    {
        readonly JwtSecurityOptions options;
        readonly SymmetricSecurityKey symmetricSecurityKey;

        public TokenHandler(JwtSecurityOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));

            this.symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.IssuerSigningKey));
        }
        
        public virtual string GenerateToken(IEnumerable<Claim> claims, DateTime? notBefore, DateTime? expires)
        {
            var jwt = new JwtSecurityToken(
              issuer: options.Issuer,
              claims: claims,
              notBefore: notBefore,
              expires: expires,
              signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            var result = new JwtSecurityTokenHandler()
                .WriteToken(jwt);

            return result;
        }

        public virtual ClaimsPrincipal ValidateToken(string token)
        {
            var result = new JwtSecurityTokenHandler()
                .ValidateToken(token, this.TokenValidationParameters, out SecurityToken validatedToken);

            return result;
        }

        public virtual TokenValidationParameters TokenValidationParameters
        {
            get
            {
                var result = new TokenValidationParameters()
                {
                    ValidIssuer = options.Issuer,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmetricSecurityKey
                };
                return result;
            }
        }
    }
}
