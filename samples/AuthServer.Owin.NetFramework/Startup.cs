using Aloji.JwtSecurity.Options;
using Aloji.Owin.JwtSecurity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuthServer.Owin.NetFramework
{
    public class Startup
    {
        const string tokenPath = "/token";

        public void Configuration(IAppBuilder appBuilder)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ConfigureOAuth(appBuilder);
        }

        private void ConfigureOAuth(IAppBuilder appBuilder)
        {
            appBuilder.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(tokenPath),
                Provider = new OAuthAuthorizationServerProvider
                {
                    OnValidateClientAuthentication = async (context) => {
                        context.Validated();
                        await Task.FromResult(0);
                    },
                    OnGrantResourceOwnerCredentials = async (context) =>
                    {
                        if (context.UserName != context.Password)
                        {
                            context.Rejected();
                            return;
                        }

                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim(ClaimTypes.Surname, context.UserName));

                        context.Validated(identity);
                        await Task.FromResult(0);
                    }
                },
                AccessTokenExpireTimeSpan = new System.TimeSpan(1, 0, 0),
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