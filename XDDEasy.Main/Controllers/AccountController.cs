using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using Common.Helper;
using XDDEasy.Contract;
using XDDEasy.Contract.AccountContract;
using XDDEasy.Domain.Identity;
using XDDEasy.Domain.ResourceAggregates;
using Common.Models;
using Common.Mvc;
using log4net;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RequestContext = Common.Models.RequestContext;
using XDDEasy.Domain.AccountAggregates;
using XDDEasy.Main.Models;
using XDDEasy.Domain.EmailAggregates;

namespace XDDEasy.Main.Controllers
{
    [Authorize]
    public class AccountController : EasyMvcBaseController
    {
        private EasySignInManager _signInManager;
        private EasyUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IEqlHttpClient _client;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;
        private readonly RequestContext _requestContext;
        private readonly ILog _log;

        //
        public AccountController(
            EasyUserManager userManager,
            EasySignInManager signInManager,
            IAuthenticationManager authenticationManager,
            IEqlHttpClient client,
            IAccountService accountService,
            IEmailService emailService,
            RequestContext requestContext, ILog log
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _client = client;
            _emailService = emailService;
            _accountService = accountService;
            _requestContext = requestContext;
            _log = log;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userManager.FindByName(model.UserName);

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:

                    //获取外部登录信息
                    var loginInfo = _authenticationManager.GetExternalLoginInfo();
                    if (loginInfo != null)
                    {
                        //添加外部登录信息
                        _userManager.AddLogin(user.Id, loginInfo.Login);
                    }

                    return GetActionResultAfterAuthenticated(model.UserName);
                    //return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "无效的登录尝试。");
                    return View(model);
            }
        }

        private ActionResult GetActionResultAfterAuthenticated(string userName)
        {
            List<string> roles = null;
            string userId;
            if (!string.IsNullOrEmpty(userName))
            {
                var user = _userManager.FindByName(userName);
                roles = _userManager.GetRoles(user.Id).ToList();
                userId = user.Id;
            }
            else
            {

                userId = ClaimHelper.GetClaimValue(EasyClaimType.UserId);
                var roleString = ClaimHelper.GetClaimValue(EasyClaimType.Roles);
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleString))
                {
                    //get socail login info
                    var claimIdentity = _authenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                    if (claimIdentity != null)
                    {
                        var idClaim = claimIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                        if (idClaim != null)
                        {
                            var user = _userManager.Find(new UserLoginInfo(idClaim.Issuer, idClaim.Value));
                            if (user != null)
                            {
                                userId = user.Id;
                                roles = _userManager.GetRoles(user.Id).ToList();
                            }
                        }
                    }
                }
                if (roleString != null && !string.IsNullOrEmpty(roleString))
                {
                    roles = roleString.Split(',').ToList();
                }
            }
            if (roles != null && roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    switch (role)
                    {
                        case "SystemMember":
                            //return RedirectToAction("Sessions", "Answer", new { area = "Parent", studentId = userId });
                            // _authenticationManager.SignOut();
                            //ModelState.AddModelError("", "Student can't login portal");
                            return View("Index", new LoginViewModel());

                            //return RedirectToAction("Index", "Session", new { area = "Student" });

                        //case "RegisteredTeacher":
                        //    var eu1 = _userManager.FindById(userId);
                        //    if (!eu1.EmailConfirmed)
                        //    {
                        //        _authenticationManager.SignOut();
                        //        return RedirectToAction("SendConfimEmail", new SendAccountEmailViewModel
                        //        {
                        //            UserId = eu1.Id,
                        //            UserName = eu1.UserName,
                        //            FirstName = eu1.FirstName,
                        //            Email = eu1.Email
                        //        });
                        //    }
                        //    return RedirectToAction("Index", "Dashboard", new { area = "Teacher", p = 1 });
                        case "SchoolAdmin":
                        case "System":
                            return RedirectToAction("Index", "Question", new { area = "Teacher" });

                        default:
                            return View("Index", new LoginViewModel());
                            //var pages =
                            //    _client.GetAsyncContent<IList<RolePageResponse>>("api/page/role/" + string.Join(",", roles));
                            //if (pages != null && pages.Any())
                            //{
                            //    var firstPage = pages.First();
                            //    return RedirectToAction(firstPage.ActionName, firstPage.ControlName, new { area = firstPage.Area, p = 1 });
                            //}
                            break;
                    }
                }
            }

            _authenticationManager.SignOut();
            ModelState.AddModelError("", EasyResource.Value("UI_ValMsg_CannotLogin"));
            return View("Index", new LoginViewModel());
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // 要求用户已通过使用用户名/密码或外部登录名登录
            if (!await _signInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
            // 如果用户输入错误代码的次数达到指定的次数，则会将
            // 该用户帐户锁定指定的时间。
            // 可以在 IdentityConfig 中配置帐户锁定设置
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "代码无效。");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    TrueName = model.TrueName,
                    TwoPassword = model.TwoPassword,
                    PhoneNumber = model.PhoneNumber,
                    KeyString = "1",
                    Sex = Convert.ToInt32(model.Sex),
                    CardNo = model.CardNo,
                    BirthDay = Convert.ToDateTime(model.BirthDay),
                    DateApproved = Convert.ToDateTime("1900-1-1"),
                    Address = model.Address,
                    ParentNumber = model.ParentNumber,
                    RecNumber = model.RecNumber,
                    AgentNumber = model.AgentNumber,
                    DateAdded = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    AddedBy = new Guid("00000000-0000-0000-0000-000000000000"),
                    UpdatedBy = new Guid("00000000-0000-0000-0000-000000000000")
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //添加角色关系
                    await _userManager.AddToRoleAsync(user.Id, "SystemMember");
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                    // 发送包含此链接的电子邮件
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">這裏</a>来确认你的帐户");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // 请不要显示该用户不存在或者未经确认
                    return View("ForgotPasswordConfirmation");
                }

                // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                // 发送包含此链接的电子邮件
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "重置密码", "请通过单击 <a href=\"" + callbackUrl + "\">此处</a>来重置你的密码");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // 请不要显示该用户不存在
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // 生成令牌并发送该令牌
            if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 如果用户已具有登录名，则使用此外部登录提供程序将该用户登录
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // 如果用户没有帐户，则提示该用户创建帐户
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}