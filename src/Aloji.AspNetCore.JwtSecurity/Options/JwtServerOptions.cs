using Aloji.AspNetCore.JwtSecurity.Services.Contracts;
using Aloji.JwtSecurity.Options;
using System;

namespace Aloji.AspNetCore.JwtSecurity.Options
{
    public class JwtServerOptions : JwtSecurityOptions
    {
        public IAuthorizationServerProvider AuthorizationServerProvider { get; set; }
        public TimeSpan AccessTokenExpireTimeSpan { get; set; }
        public string TokenEndpointPath { get; set; }
    }
}
