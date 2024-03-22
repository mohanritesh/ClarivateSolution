using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace ClarivateApp.Authentication.Basic
{
    /// <summary>
    /// added the username and Password as properties
    /// </summary>
    public class BasicAuthenticationOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
       
    }
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly string _username;
        private readonly string _password;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IOptionsMonitor<BasicAuthenticationOptions> basicAuthentication) : base(options, logger, encoder, clock)
        {
            _username = basicAuthentication.CurrentValue.Username;
            _password = basicAuthentication.CurrentValue.Password;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {

                var authorizationHeader = Request.Headers["Authorization"].ToString();
                if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    return AuthenticateResult.Fail("Authorization header does not start with 'Basic '");

                }
                var authBase64Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase)));
                var credentials = authBase64Decoded.Split(':');
                var username = credentials[0];
                var password = credentials[1];

                if (username != _username || password != _password)
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }

}
