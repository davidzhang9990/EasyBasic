using Common.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using XDDEasy.Domain.Identity;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using XDDEasy.Contract.AccountContract;
using AutoMapper;
using XDDEasy.Domain.ResourceAggregates;
using System.Web.Http.OData.Query;

namespace XDDEasy.Domain.AccountAggregates
{
    public interface IAccountService
    {
        void ChangePassword(string userName, string password);

        UserResponse Login(string userName, string password);

        User GetUserByUserNameOrThrow(string userName);

        IEnumerable<UserResponse> GetPagingUsers(ODataQueryOptions<User> options);

        void Logout();

        string GetUserName();
    }

    public class AccountService : IAccountService
    {
        private readonly RequestContext _requestContext;
        private readonly EasyUserManager _userManager;
        private readonly EasySignInManager _signInManager;
        private readonly EasyRoleManager _roleManager;

        public AccountService(RequestContext requestContext,EasyUserManager userManager,EasySignInManager signInManager,EasyRoleManager roleManager)
        {
            _requestContext = requestContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public UserResponse Login(string userName, string password)
        {
            var user = _userManager.FindByName(userName);
            if (user == null)
                throw new CustomException((int)EasyStatusCode.TheUserNotExist, string.Format(EasyResource.Value("Exception_UserNotExist"), userName));
            if (!user.ActiveFlag)
                throw new CustomException((int)EasyStatusCode.InvalidAccount, EasyResource.Value("Exception_InvalidAccount"));

            var result = _signInManager.PasswordSignIn(userName, password, false, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return Mapper.Map<UserResponse>(GetUserByUserNameOrThrow(userName));
                case SignInStatus.LockedOut:
                    throw new CustomException((int)EasyStatusCode.UserLocked, string.Format(EasyResource.Value("Exception_UserLocked"), userName));
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                //case SignInStatus.Failure:
                default:
                    throw new CustomException((int)EasyStatusCode.IncorrectUserNamePassword, EasyResource.Value("Exception_Incorrect_UserName_Password"));
            }
        }

        public void ChangePassword(string userName, string password)
        {
            var user = GetUserByUserNameOrThrow(userName);
            if (user != null)
            {
                //var hashedNewPassword = _userManager.PasswordHasher.HashPassword(password);
                //await _userStore.SetPasswordHashAsync(user,hashedNewPassword);
                //await _userStore.UpdateAsync(user);
                
                var result = _userManager.ChangePassword(user.Id, user.PasswordHash, password);
                if (!result.Succeeded)
                {
                    throw new BadRequestException(EasyResource.Value("Exception_CannotChangePassword"));
                }
            }
        }

        public User GetUserByUserNameOrThrow(string userName)
        {
            var user = _userManager.FindByName(userName);
            if (user == null || (user != null && !user.ActiveFlag))
            {
                throw new NotFoundException(string.Format(EasyResource.Value("Exception_UserNotExists"), userName));
            }
            user.GetUserRoles(_userManager);
            return user;
        }

        public IEnumerable<UserResponse> GetPagingUsers(ODataQueryOptions<User> options)
        {
            var userQuery = _userManager.Users.Where(x => x.ActiveFlag);
            var ulist = options.ApplyTo(userQuery) as IEnumerable<User>;
            return Mapper.Map<IEnumerable<User>, IEnumerable<UserResponse>>(ulist);
        }

        public void Logout()
        {
            _signInManager.AuthenticationManager.SignOut();
        }

        public string GetUserName()
        {
            return "test";
        }
    }
}
