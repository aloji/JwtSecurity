using Aloji.JwtSecurity.Options;
using Aloji.Owin.JwtSecurity;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Web.Http;

namespace ResourceServer.NetFramewok
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
  
            this.ConfigureOAuth(appBuilder);
        }

        private void ConfigureOAuth(IAppBuilder appBuilder)
        {
            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = new JwtSecureDataFormat(
                    new JwtSecurityOptions
                    {
                        Issuer = "yourIssuerCode",
                        IssuerSigningKey = "yourIssuerSigningKeyCode"
                    })
            });
        }
    }
}