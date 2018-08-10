using Aloji.AspNetCore.JwtSecurity.Services.Contracts;
using Aloji.AspNetCore.JwtSecurity.Services.Implementations;
using Aloji.JwtSecurity.Options;
using System;

namespace Aloji.AspNetCore.JwtSecurity.Options
{
    public class JwtServerOptions : JwtSecurityOptions
    {
        public JwtServerOptions()
        {
            this.AuthorizationServerProvider = new AuthorizationServerProvider();
        }

        public IAuthorizationServerProvider AuthorizationServerProvider { get; set; }
        public TimeSpan AccessTokenExpireTimeSpan { get; set; }
        public string TokenEndpointPath { get; set; }
    }
}
