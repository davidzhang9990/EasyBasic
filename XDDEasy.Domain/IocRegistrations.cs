using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using XDDEasy.Domain.Identity;
using XDDEasy.Domain.Identity.Provider;
using Common.EntityFramework.DataAccess;
using Common.Ioc;
using Common.Log;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using XDDEasy.Domain.LogAggregates;
using XDDEasy.Domain.UserLogger;
using XDDEasy.Domain.ProfileAggregates;
using XDDEasy.Domain.ResourceAggregates;
using XDDEasy.Domain.EmailAggregates;
using Common.Helper;
using Common.Models;
using XDDEasy.Domain.AccountAggregates;
using XDDEasy.Domain.PageAggregates;

namespace XDDEasy.Domain
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            //注册Resiter
            RegisterIdentity(containerBuilder);

            containerBuilder.Register(context => EasyDbContext.Create())
                .As<DbContext>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            containerBuilder.RegisterGeneric(typeof(GenericRepository<>))
               .As(typeof(IRepository<>))
               .InstancePerLifetimeScope()
               .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            containerBuilder.Register(context =>
            {
                var requestContext = RequestContext.GetFromCallContext();
                if (requestContext == null)
                {
                    requestContext = new RequestContext();
                    var userId = ClaimHelper.GetClaimValue(EasyClaimType.UserId);
                    var roles = ClaimHelper.GetClaimValue(EasyClaimType.Roles);
                    var roleIds = ClaimHelper.GetClaimValue(EasyClaimType.RoleIds);
                    var userName = ClaimHelper.GetClaimValue(EasyClaimType.UserName);
                    requestContext.UserId = string.IsNullOrEmpty(userId) ? Guid.Empty : new Guid(userId);
                    requestContext.UserName = userName;
                    if (string.IsNullOrEmpty(roles))
                        requestContext.Roles = new List<EnumRole>();
                    else
                    {
                        var roleArray = roles.Split(',');
                        requestContext.Roles =
                            roleArray.Select(x => (EnumRole)Enum.Parse(typeof(EnumRole), x.ToString(), true)).ToList();
                    }
                    requestContext.RoleIds = string.IsNullOrEmpty(roleIds) ? new List<string>() : roleIds.Split(',').ToList();
                    /*requestContext.HostAdress = HttpContext.Current == null
                        ? string.Empty
                        : HttpContext.Current.Request.GetDomain();*/
                    RequestContext.SetToCallContext(requestContext);
                }
                return requestContext;
            }).InstancePerLifetimeScope();

            containerBuilder.RegisterType<AccountService>()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .EnableLog()
                .InstancePerLifetimeScope();

            RegisterLog(containerBuilder);

            RegisterProfile(containerBuilder);

            RegisterResource(containerBuilder);

            RegisterPage(containerBuilder);

            RegisterEmail(containerBuilder);

            RegisterEmailTemplate(containerBuilder);
            // User Log
            RegisterUserLog(containerBuilder);
        }

        private void RegisterIdentity(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(context =>
            {
                var userStore = context.Resolve<EasyUserStore>();
                var option = context.Resolve<IdentityFactoryOptions<EasyUserManager>>();
                return new EasyUserManager(userStore, option);
            }).As<EasyUserManager>()
                .InstancePerLifetimeScope();
            containerBuilder.Register(context =>
            {
                var userManager = context.Resolve<EasyUserManager>();
                var auth = context.Resolve<IAuthenticationManager>();
                return new EasySignInManager(userManager, auth);
            }).As<EasySignInManager>()
                .InstancePerLifetimeScope();

            containerBuilder.Register(context =>
            {
                var dbContext = context.Resolve<DbContext>();
                return new EasyRoleStore(dbContext);
            }).As<EasyRoleStore>()
                .InstancePerLifetimeScope();

            containerBuilder.Register(context =>
            {
                var roleStore = context.Resolve<EasyRoleStore>();
                return new EasyRoleManager(roleStore);
            }).As<EasyRoleManager>()
                .InstancePerLifetimeScope();

            containerBuilder
              .RegisterType<ApplicationOAuthProvider>()
              .As<IOAuthAuthorizationServerProvider>()
              .PropertiesAutowired()
              .InstancePerLifetimeScope();

            containerBuilder.Register(context => HttpContext.Current == null ? null : HttpContext.Current.GetOwinContext().Authentication)
                .As<IAuthenticationManager>()
                .InstancePerLifetimeScope();

            containerBuilder.Register(context =>
            {
                var dbContext = context.Resolve<DbContext>();
                return new EasyUserStore(dbContext);
            }).As<EasyUserStore>()
                .InstancePerLifetimeScope();

            containerBuilder.Register(
                context => new IdentityFactoryOptions<EasyUserManager>()
                {
                    DataProtectionProvider =
                        new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("Equlaerning")
                }).As<IdentityFactoryOptions<EasyUserManager>>()
                .InstancePerLifetimeScope();
        }

        private void RegisterLog(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<LogRepository>()
             .AsImplementedInterfaces()
             .EnableInterfaceInterceptors()
             .EnableLog()
             .InstancePerLifetimeScope()
             .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            containerBuilder.RegisterType<LogService>()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .EnableLog()
                .InstancePerLifetimeScope();
        }

        private void RegisterProfile(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ProfileService>()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .EnableLog()
                .InstancePerLifetimeScope();
        }

        private void RegisterResource(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ResourceProvider>()
                .As<IResourceProvider>()
                .SingleInstance();
        }

        private void RegisterPage(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<PageService>()
              .AsImplementedInterfaces()
              .EnableInterfaceInterceptors()
              .EnableLog()
              .InstancePerLifetimeScope();
        }

        private void RegisterEmail(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<EmailRepository>().AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            containerBuilder.RegisterType<XDDEasy.Domain.EmailAggregates.EmailService>()
              .AsImplementedInterfaces()
              .EnableInterfaceInterceptors()
              .EnableLog()
              .InstancePerLifetimeScope();


            if (WebConfigurationManager.GetBoolean("UseSmtpClient"))
            {
                containerBuilder.RegisterType<SmtpMailService>()
                    .AsImplementedInterfaces()
                    .EnableInterfaceInterceptors()
                    .EnableLog()
                    .InstancePerLifetimeScope();
            }
            else
            {
                containerBuilder.RegisterType<SendGridEmailService>()
                    .AsImplementedInterfaces()
                    .EnableInterfaceInterceptors()
                    .EnableLog()
                    .InstancePerLifetimeScope();
            }
        }

        private void RegisterEmailTemplate(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<EmailTemplateService>()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .EnableLog()
                .InstancePerLifetimeScope();
        }

        private void RegisterUserLog(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UserLogRepository>()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .EnableLog()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            containerBuilder.RegisterType<UserLogServices>()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .EnableLog()
                .InstancePerLifetimeScope();
        }
    }
}
