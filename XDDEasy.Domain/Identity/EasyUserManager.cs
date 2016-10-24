using System;
using System.Web.Security;
using XDDEasy.Domain.AccountAggregates;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace XDDEasy.Domain.Identity
{
    public sealed class EasyUserManager : UserManager<User>
    {
        public EasyUserManager(EasyUserStore store, IdentityFactoryOptions<EasyUserManager> options)
            : base(store)
        {
            // 配置用户名的验证逻辑
            UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordHasher = new IdentityPasswordHasher();
            PasswordValidator = new PasswordValidator();
            UserValidator = new IdentityUserValidation();

            //// 配置密码的验证逻辑
            //manager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};
            // 配置用户锁定默认值
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并将其插入到此处。
            RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "你的安全代码是 {0}"
            });
            RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<User>
            {
                Subject = "安全代码",
                BodyFormat = "你的安全代码是 {0}"
            });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            /*  var dataProtectionProvider = options.DataProtectionProvider;
              if (dataProtectionProvider != null)
              {
                  UserTokenProvider =
                      new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
              }*/
            var provider = new MachineKeyProtectionProvider();
            UserTokenProvider = new DataProtectorTokenProvider<User>(
                provider.Create("ASP.NET Identity"));
        }
    }

    public class MachineKeyProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes)
        {
            return new MachineKeyDataProtector(purposes);
        }
    }

    public class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        public MachineKeyDataProtector(string[] purposes)
        {
            _purposes = purposes;
        }

        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purposes);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purposes);
        }
    }
}
