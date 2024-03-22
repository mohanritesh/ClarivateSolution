using Microsoft.AspNetCore.Authorization;

namespace ClarivateApp.Authentication.Basic
{
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {
        public BasicAuthorizationAttribute()
        {
            AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme;
        }
    }

   
}
