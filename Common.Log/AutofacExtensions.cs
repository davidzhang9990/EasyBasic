using Autofac.Builder;
using Autofac.Extras.DynamicProxy2;

namespace Common.Log
{
    public static class AutofacExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> EnableLog<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration)
        {
            return registration.InterceptedBy(typeof(PublicInterfaceLoggingInterceptor));
        }
    }
}
