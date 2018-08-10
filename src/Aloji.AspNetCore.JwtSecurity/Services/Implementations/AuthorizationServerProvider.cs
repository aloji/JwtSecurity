using Aloji.AspNetCore.JwtSecurity.Context;
using Aloji.AspNetCore.JwtSecurity.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace Aloji.AspNetCore.JwtSecurity.Services.Implementations
{
    public class AuthorizationServerProvider : IAuthorizationServerProvider
    {
        public Func<GrantResourceOwnerCredentialsContext, Task> OnGrantResourceOwnerCredentialsAsync { get; set; }

        public AuthorizationServerProvider()
        {
            this.OnGrantResourceOwnerCredentialsAsync = context => Task.FromResult<object>(null);
        }

        public virtual Task GrantClientCredentialsAsync(GrantResourceOwnerCredentialsContext context)
        {
            return this.OnGrantResourceOwnerCredentialsAsync.Invoke(context);
        }
    }
}
