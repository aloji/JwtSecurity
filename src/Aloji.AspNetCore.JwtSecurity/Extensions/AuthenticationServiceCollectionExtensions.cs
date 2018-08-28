using Aloji.JwtSecurity.Options;
using Aloji.JwtSecurity.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, Action<JwtSecurityOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var jwtSecurityOptions = new JwtSecurityOptions();
            configureOptions(jwtSecurityOptions);

            var tokenHandler = new TokenHandler(jwtSecurityOptions);

            services.AddAuthentication(
                options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenHandler.TokenValidationParameters;
                });

            return services;
        }
    }
}
