using Aloji.JwtSecurity.Options;
using Aloji.JwtSecurity.Services.Contracts;
using Aloji.JwtSecurity.Services.Implementations;
using Microsoft.Owin.Security;
using System;
using System.Linq;

namespace Aloji.Owin.JwtSecurity
{
    public class JwtSecureDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        readonly ITokenHandler iTokenHandler;

        public JwtSecureDataFormat(ITokenHandler iTokenHandler)
        {
            this.iTokenHandler = iTokenHandler ?? throw new ArgumentNullException(nameof(iTokenHandler));
        }

        public JwtSecureDataFormat(JwtSecurityOptions jwtSecurityOptions)
            : this(new TokenHandler(jwtSecurityOptions))
        {
        }

        public virtual string Protect(AuthenticationTicket data)
        {
            var notBefore = data.Properties.IssuedUtc.HasValue ?
                data.Properties.IssuedUtc.Value.UtcDateTime : default(DateTime?);

            var expires = data.Properties.ExpiresUtc.HasValue ?
                data.Properties.ExpiresUtc.Value.UtcDateTime : default(DateTime?);

            var result = this.iTokenHandler.GenerateToken(data.Identity.Claims, notBefore, expires);
            return result;
        }

        public virtual AuthenticationTicket Unprotect(string protectedText)
        {
            var result = default(AuthenticationTicket);
            var principal = this.iTokenHandler.ValidateToken(protectedText);
            if (principal != null && principal.Identities != null && principal.Identities.Any())
            {
                result = new AuthenticationTicket(principal.Identities.First(), new AuthenticationProperties());
            }
            return result;
        }
    }
}