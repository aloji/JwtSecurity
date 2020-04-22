using Aloji.AspNetCore.JwtSecurity.Constants;
using Aloji.AspNetCore.JwtSecurity.Context;
using Aloji.AspNetCore.JwtSecurity.Options;
using Aloji.AspNetCore.JwtSecurity.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Aloji.AspNetCore.JwtSecurity.Middleware
{
    public class JwtServerMiddleware
    {
        readonly JwtServerOptions jwtServerOptions;

        public JwtServerMiddleware(RequestDelegate next, JwtServerOptions jwtServerOptions)
        {
            this.jwtServerOptions = jwtServerOptions ?? throw new ArgumentNullException(nameof(jwtServerOptions));
        }

        public async Task InvokeAsync(HttpContext context, IJwtSecurityTokenService iJwtSecurityTokenService)
        {
            var baseValidatingContext = default(BaseValidatingContext);
            var grantType = this.GetGrantType(context);

            switch (grantType)
            {
                case Parameters.Password:
                    baseValidatingContext = GrantResourceOwnerCredentialsContext.Create(context);
                    if(baseValidatingContext != null)
                        await jwtServerOptions.AuthorizationServerProvider.GrantClientCredentialsAsync
                            ((GrantResourceOwnerCredentialsContext)baseValidatingContext);
                    break;
                case Parameters.RefreshToken:
                    baseValidatingContext = GrantRefreshTokenContext.Create(context);
                    if (baseValidatingContext != null)
                        await jwtServerOptions.AuthorizationServerProvider.GrantRefreshTokenAsync
                            ((GrantRefreshTokenContext)baseValidatingContext);
                    break;
                default:
                    break;
            }

            if (baseValidatingContext != null)
            {
                if (baseValidatingContext.IsValidated)
                {
                    var token = await iJwtSecurityTokenService.CreateAsync(baseValidatingContext, jwtServerOptions);
                    await WriteResponseAsync(context, JsonConvert.SerializeObject(token));
                }
                else
                {
                    await WriteResponseError(context, baseValidatingContext.Error);
                }
            }
        }

        private string GetGrantType(HttpContext context)
        {
            var result = default(string);
            var requestForm = context.Request.Form;
            if (requestForm.ContainsKey(Parameters.GrandType))
            {
                result = requestForm[Parameters.GrandType].FirstOrDefault();
            }
            return result;
        }

        private async Task WriteResponseAsync(HttpContext context, string content)
        {
            const string contentType = "application/json";

            context.Response.Headers[HeaderNames.ContentType] = contentType;
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync(content);
        }

        private async Task WriteResponseError(HttpContext context, string error)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(error);
        }
    }
}
