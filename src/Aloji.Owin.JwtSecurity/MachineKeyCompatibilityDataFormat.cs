using Aloji.JwtSecurity.Options;
using Aloji.JwtSecurity.Services.Contracts;
using Aloji.JwtSecurity.Services.Implementations;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;

namespace Aloji.Owin.JwtSecurity
{
    public class MachineKeyCompatibilityDataFormat : JwtSecureDataFormat
    {
        public MachineKeyCompatibilityDataFormat(ITokenHandler iTokenHandler)
            : base(iTokenHandler)
        {
        }

        public MachineKeyCompatibilityDataFormat(JwtSecurityOptions jwtSecurityOptions)
            : this(new TokenHandler(jwtSecurityOptions))
        {
        }

        public override AuthenticationTicket Unprotect(string protectedText)
        {
            var result = default(AuthenticationTicket);
            try
            {
                result = base.Unprotect(protectedText);
            }
            catch{ }

            if (result == null)
            {
                var secureDataFormat = new TicketDataFormat(new MachineKeyProtector());
                result = secureDataFormat.Unprotect(protectedText);
            }
            return result;
        }
    }
}