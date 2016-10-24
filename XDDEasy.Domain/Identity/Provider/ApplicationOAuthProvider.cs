using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using XDDEasy.Domain.AccountAggregates;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace XDDEasy.Domain.Identity.Provider
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";
        private const string Origin = "Origin";

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var container =
                ((AutofacWebApiDependencyResolver)GlobalConfiguration.Configuration.DependencyResolver).Container;
            var userManager = container.Resolve<EasyUserManager>();

            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            if (!user.ActiveFlag)
            {
                context.SetError("invalid_grant", "The user name is inactive.");
                return;
            }

            var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager);
            var cookiesIdentity = await user.GenerateUserIdentityAsync(userManager);

            //userManager.Update(user);//更新apikey

            var properties = CreateProperties(user, cookiesIdentity);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);

            context.Request.Context.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }


            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var isCorsRequest = context.Request.Headers.GetValues(Origin) != null && context.Request.Headers.GetValues(Origin).Count > 0;
            if (isCorsRequest)
            {
                context.Response.Headers.Remove(AccessControlAllowOrigin);
                context.Response.Headers.Add(AccessControlAllowOrigin, new[] { context.Request.Headers[Origin] });
            }

            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }


        public static AuthenticationProperties CreateProperties(User user, ClaimsIdentity identity)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                /* { "userName", user.UserName},
                 { "userId", user.Id },
                 {"role",user.UserRoles.Any()? user.UserRoles.First().ToString():""},*/
                //{"apiKey",user.ApiKey.GetValueOrDefault().ToString()}
            };

            foreach (var claim in identity.Claims)
            {
                if (claim.Type.StartsWith("easy"))
                    data.Add(claim.Type.Substring(4), claim.Value);
            }
            return new AuthenticationProperties(data);
        }
    }
}
