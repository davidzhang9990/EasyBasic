using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XDDEasy.Domain.AccountAggregates;

namespace XDDEasy.Domain.Identity
{
    public class EasySignInManager : SignInManager<User, string>
    {
        public EasySignInManager(EasyUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((EasyUserManager)UserManager);
        }

        //public static EasySignInManager Create(IdentityFactoryOptions<EasySignInManager> options, IOwinContext context)
        //{
        //    return new EasySignInManager(context.GetUserManager<EasyUserManager>(), context.Authentication);
        //}
    }
}
