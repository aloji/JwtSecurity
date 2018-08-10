using Aloji.AspNetCore.JwtSecurity.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthServer.AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddJwtServer()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseJwtServer(options => {
                options.TokenEndpointPath = "/token";
                options.AccessTokenExpireTimeSpan = new TimeSpan(1, 0, 0);
                options.Issuer = "yourIssuerCode";
                options.IssuerSigningKey = "yourIssuerSigningKeyCode";
                options.AuthorizationServerProvider = new AuthorizationServerProvider
                {
                    OnGrantResourceOwnerCredentialsAsync = async (context) =>
                    {
                        if (context.UserName != context.Password)
                        {
                            context.SetError("Invalid user authentication");
                            return;
                        }

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Surname, context.UserName)
                        };

                        context.Validated(claims);
                        await Task.FromResult(0);
                    }
                };
            });

            app.UseMvc();
        }
    }
}
