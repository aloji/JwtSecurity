using Aloji.AspNetCore.JwtSecurity.Constants;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Aloji.AspNetCore.JwtSecurity.Context
{
    public class GrantResourceOwnerCredentialsContext : BaseValidatingContext
    {
        public GrantResourceOwnerCredentialsContext(HttpContext context, string userName, string password) : base(context)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }

        public static GrantResourceOwnerCredentialsContext Create(HttpContext context)
        {
            var result = default(GrantResourceOwnerCredentialsContext);
            var requestForm = context.Request.Form;
            if (requestForm.ContainsKey(Parameters.GrandType))
            {
                var grandTypeValue = requestForm[Parameters.GrandType].FirstOrDefault();
                if (grandTypeValue == Parameters.Password)
                {
                    var userName = requestForm[Parameters.Username].FirstOrDefault();
                    var password = requestForm[Parameters.Password].FirstOrDefault();

                    result = new GrantResourceOwnerCredentialsContext(context, userName, password);
                }
            }
            return result;
        }
    }
}
