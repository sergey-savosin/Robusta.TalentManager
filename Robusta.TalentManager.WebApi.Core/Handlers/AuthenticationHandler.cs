using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security;

namespace Robusta.TalentManager.WebApi.Core.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("X-PSK"))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "jqhuman")
                };

                var principal = new ClaimsPrincipal(new[] { new ClaimsIdentity(claims, "dummy") });

                request.GetRequestContext().Principal = principal;
                Thread.CurrentPrincipal = principal;
            }

            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            Console.WriteLine("Name: {0}\nIsAuthenticated: {1}" +
                "\nAuthenticationType: {2}",
                threadPrincipal.Identity.Name,
                threadPrincipal.Identity.IsAuthenticated,
                threadPrincipal.Identity.AuthenticationType);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
