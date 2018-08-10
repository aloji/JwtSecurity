using Aloji.AspNetCore.JwtSecurity.Middleware;
using Aloji.AspNetCore.JwtSecurity.Options;
using Microsoft.AspNetCore.Http;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class JwtServerAppBuilderExtensions
    {
        const string xFormUrlEncoded = "application/x-www-form-urlencoded";

        public static IApplicationBuilder UseJwtServer(this IApplicationBuilder app, Action<JwtServerOptions> configureOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var jwtServerOptions = new JwtServerOptions();
            configureOptions(jwtServerOptions);

            app.MapWhen(context => IsValidJwtMiddlewareRequest(context, jwtServerOptions.TokenEndpointPath),
                      appBuilder => appBuilder.UseMiddleware<JwtServerMiddleware>(jwtServerOptions));

            return app;
        }

        private static bool IsValidJwtMiddlewareRequest(HttpContext context, string tokenPath)
        {
            return context.Request.Method == HttpMethods.Post &&
                   context.Request.ContentType == xFormUrlEncoded &&
                   context.Request.Path == tokenPath;
        }
    }
}
