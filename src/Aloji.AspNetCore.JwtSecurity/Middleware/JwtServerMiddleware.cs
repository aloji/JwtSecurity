using Aloji.AspNetCore.JwtSecurity.Context;
using Aloji.AspNetCore.JwtSecurity.Options;
using Aloji.AspNetCore.JwtSecurity.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
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
            var grantResourceOwnerCredentialsContext = GrantResourceOwnerCredentialsContext.Create(context);

            if (grantResourceOwnerCredentialsContext != null)
            {
                await jwtServerOptions.AuthorizationServerProvider.GrantClientCredentialsAsync(grantResourceOwnerCredentialsContext);

                if (grantResourceOwnerCredentialsContext.IsValidated)
                {
                    var token = iJwtSecurityTokenService.Create(grantResourceOwnerCredentialsContext, jwtServerOptions);
                    await WriteResponseAsync(context, JsonConvert.SerializeObject(token));
                }
                else
                {
                    await WriteResponseError(context, grantResourceOwnerCredentialsContext.Error);
                }
            }
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
