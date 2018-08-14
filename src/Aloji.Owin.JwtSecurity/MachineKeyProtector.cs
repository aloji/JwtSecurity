using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using System;

namespace Aloji.Owin.JwtSecurity
{
    public class MachineKeyProtector : IDataProtector
    {
        private readonly string[] _purpose =
        {
            typeof(OAuthAuthorizationServerMiddleware).Namespace,
            "Access_Token",
            "v1"
        };

        public byte[] Protect(byte[] userData)
        {
            throw new NotImplementedException();
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return System.Web.Security.MachineKey.Unprotect(protectedData, _purpose);
        }
    }
}
