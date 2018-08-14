using Aloji.JwtSecurity.Options;
using Aloji.JwtSecurity.Services.Contracts;
using Aloji.JwtSecurity.Services.Implementations;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;

namespace Aloji.Owin.JwtSecurity
{
    public class MachineKeyCompatibiltyDataFormat : JwtSecureDataFormat
    {
        public MachineKeyCompatibiltyDataFormat(ITokenHandler iTokenHandler)
            : base(iTokenHandler)
        {
        }

        public MachineKeyCompatibiltyDataFormat(JwtSecurityOptions jwtSecurityOptions)
            : this(new TokenHandler(jwtSecurityOptions))
        {
        }

        public override AuthenticationTicket Unprotect(string protectedText)
        {
            var result = base.Unprotect(protectedText);
            if (result == null)
            {
                var secureDataFormat = new TicketDataFormat(new MachineKeyProtector());
                result = secureDataFormat.Unprotect(protectedText);
            }
            return result;
        }
    }
}