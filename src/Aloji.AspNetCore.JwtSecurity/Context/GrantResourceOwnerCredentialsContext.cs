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
            const string grandTypeParam = "grant_type";
            const string userNameParam = "username";
            const string passwordParam = "password";

            var result = default(GrantResourceOwnerCredentialsContext);
            var requestForm = context.Request.Form;
            if (requestForm.ContainsKey(grandTypeParam))
            {
                var grandTypeValue = requestForm[grandTypeParam].FirstOrDefault();
                if (grandTypeValue == passwordParam)
                {
                    var userName = requestForm[userNameParam].FirstOrDefault();
                    var password = requestForm[passwordParam].FirstOrDefault();

                    result = new GrantResourceOwnerCredentialsContext(context, userName, password);
                }
            }
            return result;
        }
    }
}
