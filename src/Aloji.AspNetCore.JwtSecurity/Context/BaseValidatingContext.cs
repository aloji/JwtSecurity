using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Aloji.AspNetCore.JwtSecurity.Context
{
    public class BaseValidatingContext
    {
        public HttpContext Context { get; }
        public IEnumerable<Claim> Claims { get; private set; }

        public BaseValidatingContext(HttpContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public HttpRequest Request => Context.Request;

        public HttpResponse Response => Context.Response;

        /// <summary>
        /// True if application code has called any of the Validate methods on this context.
        /// </summary>
        public bool IsValidated { get; private set; }

        /// <summary>
        /// True if application code has called the SetError methods on this context.
        /// </summary>
        public bool HasError { get; private set; }

        /// <summary>
        /// The error argument provided when SetError was called on this context.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Marks this context as validated by the application. IsValidated becomes true and HasError becomes false as a result of calling.
        /// </summary>
        /// <returns>True if the validation has taken effect.</returns>
        public bool Validated(IEnumerable<Claim> claims)
        {
            this.Claims = claims;
            this.IsValidated = true;
            this.HasError = false;
            return true;
        }

        public void SetError(string error)
        {
            this.Error = error;
            this.IsValidated = false;
            this.HasError = true;
        }
    }
}
