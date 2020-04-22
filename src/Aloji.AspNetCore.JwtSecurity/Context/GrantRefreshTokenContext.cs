using Aloji.AspNetCore.JwtSecurity.Constants;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Aloji.AspNetCore.JwtSecurity.Context
{
    public class GrantRefreshTokenContext : BaseValidatingContext
    {
        public GrantRefreshTokenContext(HttpContext context, string token)
            : base(context)
        {
            this.Token = token;
        }

        public string Token { get; private set; }

        public static GrantRefreshTokenContext Create(HttpContext context)
        {
            var result = default(GrantRefreshTokenContext);
            var requestForm = context.Request.Form;
            if (requestForm.ContainsKey(Parameters.GrandType))
            {
                var grandTypeValue = requestForm[Parameters.GrandType].FirstOrDefault();
                if (grandTypeValue == Parameters.RefreshToken)
                {
                    var token = requestForm[Parameters.RefreshToken].FirstOrDefault();
                    result = new GrantRefreshTokenContext(context, token);
                }
            }
            return result;
        }
    }
}
