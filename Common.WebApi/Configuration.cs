using System;
using System.CodeDom;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.WebApi;
using Common.Helper;
using Common.Ioc;
using Common.WebApi.Attributes;
using Common.WebApi.DefaultHeaders;
using Common.WebApi.Exceptions;
using Common.WebApi.Handler;
using Common.WebApi.RequestContext;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Common.WebApi
{
    public class Configuration
    {
        public static IContainer Configure(HttpConfiguration httpConfiguration)
        {
            IContainer containerInstance = ContainerInstanceProvider.GetContainerInstance(Assembly.GetCallingAssembly());
            Configure(httpConfiguration, containerInstance);
            return containerInstance;
        }

        private static void Configure(HttpConfiguration httpConfiguration, IContainer container)
        {
            ConfiguraeGlobalCulture(httpConfiguration);
            //ConfigureCompression(httpConfiguration);
            ConfigureTracing(httpConfiguration);
            ConfigureDependencyResolver(httpConfiguration, container);
            ConfiguraeRequestLogInfo(httpConfiguration, container);
            ConfigureLogging(httpConfiguration);
            //ConfigureAuthorizationIntegration(container);
            ConfigureJsonSerializer(httpConfiguration);
            //ConfigureIdentityIntegration(httpConfiguration);
            ConfigureCors(httpConfiguration);
            //ConfigureApiVersion(httpConfiguration);
            ConfigureExceptionHandling(httpConfiguration, container);
            //ConfigureOperactorLog(httpConfiguration);
            //ConfigureSwagger(httpConfiguration);
            //ConfigureRateLimiting(httpConfiguration);
            httpConfiguration.EnsureInitialized();
            ConfigureDefaultMediaFormatter(httpConfiguration);
            //ConfigureRequestContext(httpConfiguration);
            ConfigErrorDetailPolicy(httpConfiguration);
            httpConfiguration.EnsureInitialized();
        }

        private static void ConfigureExceptionHandling(HttpConfiguration httpConfiguration, IContainer container)
        {
            var exceptionFileter = container.Resolve<CommonExceptionFilterAttribute>();
            httpConfiguration.Filters.Add(exceptionFileter);
        }

        private static void ConfigureRequestContext(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MessageHandlers.Add(new RegisterRequestContextHandler());
        }

        private static void ConfigureDependencyResolver(HttpConfiguration httpConfiguration, IContainer container)
        {
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        /// <summary>
        /// config tracing
        /// </summary>
        /// <param name="httpConfiguration"></param>
        private static void ConfigureTracing(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.EnableSystemDiagnosticsTracing();
        }

        /// <summary>
        /// config compression
        /// </summary>
        /// <param name="httpConfiguration"></param>
        private static void ConfigureCompression(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MessageHandlers.Insert(0, new CompressionHandler()); // first runs last
        }

        private static void ConfigureOperactorLog(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MessageHandlers.Add(new OperatorLogHandler());
        }

        private static void ConfiguraeGlobalCulture(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MessageHandlers.Add(new LanguageMessageHandler());
        }
        private static void ConfiguraeRequestLogInfo(HttpConfiguration httpConfiguration, IContainer iContainer)
        {
            httpConfiguration.MessageHandlers.Add(new UserLogHandler(iContainer));
        }
        /// <summary>
        /// config formatter
        /// </summary>
        /// <param name="httpConfiguration"></param>
        private static void ConfigureJsonSerializer(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            var jsonMediaTypeFormatter = httpConfiguration.Formatters.JsonFormatter;
            jsonMediaTypeFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            jsonMediaTypeFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonMediaTypeFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonMediaTypeFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            jsonMediaTypeFormatter.SerializerSettings.Converters.Add(new VersionConverter());
#if (DEBUG)
            jsonMediaTypeFormatter.SerializerSettings.Formatting = Formatting.Indented;
#endif
        }


        public static void ConfigErrorDetailPolicy(HttpConfiguration httpConfiguration)
        {
            var policy = WebConfigurationManager.GetValue("IncludeErrorDetailPolicy");
            if (string.IsNullOrEmpty(policy))
                policy = "Never";
            httpConfiguration.IncludeErrorDetailPolicy =
                (IncludeErrorDetailPolicy)Enum.Parse(typeof(IncludeErrorDetailPolicy), policy);
        }


        /// <summary>
        /// configure logger
        /// </summary>
        /// <param name="httpConfiguration"></param>
        private static void ConfigureLogging(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Filters.Add((IFilter)new LoggingAttribute(LogManager.GetLogger(typeof(LoggingAttribute).Name)));
        }


        private static void ConfigureDefaultMediaFormatter(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MessageHandlers.Add(new DeafultContentTypeHandler());
        }

        //configure Cors
        private static void ConfigureCors(HttpConfiguration config)
        {
            #region configuration option

            // this allows http://foo.com to do GET or POST on Values1 controller
            //corsConfig
            //    .ForResources(“Values1″)
            //    .ForOrigins(“http://foo.com&#8221;)
            //    .AllowMethods(“GET”, “POST”);

            //// this allows http://foo.com to do GET and POST, pass cookies and
            //// read the Foo response header on Values2 controller
            //corsConfig
            //    .ForResources(“Values2″)
            //    .ForOrigins(“http://foo.com&#8221;)
            //    .AllowMethods(“GET”, “POST”)
            //    .AllowCookies()
            //    .AllowResponseHeaders(“Foo”);

            //// this allows http://foo.com and http://foo.com to do GET, POST,
            //// and PUT and pass the Content-Type header to Values3 controller
            //corsConfig
            //    .ForResources(“Values3″)
            //    .ForOrigins(“http://foo.com&#8221;, “http://bar.com&#8221;)
            //    .AllowMethods(“GET”, “POST”, “PUT”)
            //    .AllowRequestHeaders(“Content-Type”);

            //// this allows http://foo.com to use any method, pass cookies, and
            //// pass the Content-Type, Foo and Authorization headers, and read
            //// the Foo response header for Values4 and Values5 controllers
            //corsConfig
            //    .ForResources(“Values4″, “Values5″)
            //    .ForOrigins(“http://foo.com&#8221;)
            //    .AllowAllMethods()
            //    .AllowCookies()
            //    .AllowRequestHeaders(“Content-Type”, “Foo”, “Authorization”)
            //    .AllowResponseHeaders(“Foo”);

            //// this allows all methods and all request headers (but no cookies)
            //// from all origins to Values6 controller
            //corsConfig
            //    .ForResources(“Values6″)
            //    .AllowAllOriginsAllMethodsAndAllRequestHeaders();

            //// this allows all methods (but no cookies or request headers)
            //// from all origins to Values7 controller
            //corsConfig
            //    .ForResources(“Values7″)
            //    .AllowAllOriginsAllMethods();

            //// this allows all CORS requests from origin http://bar.com
            //// for all resources that have not been explicitly configured
            //corsConfig
            //    .ForOrigins(“http://bar.com&#8221;)
            //    .AllowAll();

            #endregion

            //// this allows all CORS requests to all other resources that don’t
            //// have an explicit configuration. This opens them to all origins, all
            //// HTTP methods, all request headers and cookies. This is the API to use
            //// to get started, but it’s a sledgehammer in the sense that *everything*
            //// is wide-open.
            //var corsConfig = new WebApiCorsConfiguration();
            //corsConfig.RegisterGlobal(config);
            //corsConfig
            //   .ForAllResources()
            //   .ForAllOrigins()
            //   .AllowAllMethods()
            //   .AllowAllRequestHeaders()
            //   .AllowAll();

            //var corsAttr = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors();

            config.MessageHandlers.Add(new Common.WebApi.Cors.CorsMessageHandler());
        }
    }
}
