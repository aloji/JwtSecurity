using Aloji.AspNetCore.JwtSecurity.Services.Contracts;
using Aloji.AspNetCore.JwtSecurity.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class JwtServerServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtServer(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IAuthorizationServerProvider, AuthorizationServerProvider>();
            services.AddSingleton<IJwtSecurityTokenService, JwtSecurityTokenService>();

            return services;
        }
    }
}
