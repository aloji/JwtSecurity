# JwtSecurity

The object is to allow using a token generated in an OWIN OAuth 2.0 Server in AspNet.Core projects.

#### Nugets: https://www.nuget.org/profiles/aloji

## Real life problem

We have the authorization server implemented with [OWIN OAuth 2.0](https://docs.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/owin-oauth-20-authorization-server), but the new developments are with AspNetCore


The first idea was to use the machine keys

###### MachineKey

> If the authorization server and the resource server are not on the same computer, the OAuth middleware will use the different machine keys to encrypt and decrypt bearer access token. In order to share the same private key between both projects, we add the same machinekey setting in both web.config files.

The problem is that machinekey does not exist in AspNetCore, but MS gives us a [compatibility solution](https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/compatibility/replacing-machinekey?view=aspnetcore-2.1) to replace the machinekey settings in AspNetWebApi2 and using a [key storge provider](https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-storage-providers?view=aspnetcore-2.1) like redis we can be shared the keys.

After several hours trying to implement this solution, I realized that it was easier, cleaner and cheaper to change the token generator to use JWT and dont use any external provider.


## Configuration

How to setup the JwtSecurity in OWIN OAuth 2.0 Server ([full sample code](https://github.com/aloji/JwtSecurity/blob/master/samples/AuthServer.Owin.NetFramework/Startup.cs))

```csharp
public class Startup
{
    public void Configuration(IAppBuilder appBuilder)
    {
        appBuilder.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
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
```

How to setup the JwtSecurity in Resource Server .NetFramework ([full sample code](https://github.com/aloji/JwtSecurity/blob/master/samples/ResourceServer.NetFramewok/Startup.cs))

```csharp
public class Startup
{
    public void Configuration(IAppBuilder appBuilder)
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
```

How to setup the JwtSecurity in Resource Server .NetFramework with Owin Auth Compatibility

```csharp
public class Startup
{
    public void Configuration(IAppBuilder appBuilder)
    {
        appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
        {
            AccessTokenFormat = new MachineKeyCompatibilityDataFormat(
                new JwtSecurityOptions
                {
                    Issuer = "yourIssuerCode",
                    IssuerSigningKey = "yourIssuerSigningKeyCode"
                })
        });
    }
}
```

How to setup the JwtSecurity in Resource Server .NetCore ([full sample code](https://github.com/aloji/JwtSecurity/blob/master/samples/ResourceServer.AspNetCore/Startup.cs))

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
              .AddJwtBearerAuthentication(options =>
              {
                  options.Issuer = "yourIssuerCode";
                  options.IssuerSigningKey = "yourIssuerSigningKeyCode";
              });
    }
    
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
          app.UseAuthentication();
    }
}
```

#### Bonus:

I developed a middleware to create a JwtServer with AspNetCore very similar to OwinOAuth2 settings

How to setup the JwtServer in AspNetCore ([full sample code](https://github.com/aloji/JwtSecurity/blob/master/samples/AuthServer.AspNetCore/Startup.cs))

```csharp
 public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddJwtServer();
    }
    
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
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
    }
}
```

