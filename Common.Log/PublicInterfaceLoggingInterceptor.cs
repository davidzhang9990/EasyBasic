using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using log4net;

namespace Common.Log
{
    public class PublicInterfaceLoggingInterceptor : IInterceptor
    {
        private readonly ILog _log;

        public PublicInterfaceLoggingInterceptor(ILog log)
        {
            Contract.Requires(log != null);

            _log = log;
        }

        public void Intercept(IInvocation invocation)
        {
            var isDebugEnabled = _log.IsDebugEnabled;
            var typeName = new Lazy<string>(() => invocation.TargetType.Name);
            var methodName = new Lazy<string>(() => invocation.GetConcreteMethod().Name);
            var arguments = new Lazy<string>(() => FormatArguments(invocation));

            if (isDebugEnabled)
            {
                _log.DebugFormat("{0}.{1}({2})", typeName.Value, methodName.Value, arguments.Value);
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                invocation.Proceed();
            }
            catch (Exception exception)
            {
                stopwatch.Stop();
                _log.ErrorFormat("{0}.{1}({2}) -> threw exception {3}: {4} [{5} ms]", typeName.Value,
                                      methodName.Value, arguments.Value, FormatForLog(exception), exception.Message,
                                      stopwatch.ElapsedMilliseconds);
                throw;
            }

            if (isDebugEnabled)
            {
                stopwatch.Stop();
                _log.DebugFormat("{0}.{1}({2}) -> {3} [{4} ms]", typeName.Value, methodName.Value, arguments.Value,
                                      FormatForLog(invocation.ReturnValue), stopwatch.ElapsedMilliseconds);
            }

        }

        private static string FormatArguments(IInvocation invocation)
        {
            Contract.Requires(invocation != null);

            var formattedArguments = new List<string>();

            var methodInfo = invocation.GetConcreteMethod();
            var instanceParameters = methodInfo.GetParameters();
            var parameterCount = instanceParameters.Count();

            for (var parameterIndex = 0; parameterIndex < parameterCount; parameterIndex++)
            {
                var instanceParameter = instanceParameters[parameterIndex];

                if (instanceParameter.GetCustomAttributes(typeof(DoNotLogAttribute), false).Any())
                {
                    formattedArguments.Add(@"""******""");
                }
                else
                {
                    formattedArguments.Add(FormatForLog(invocation.GetArgumentValue(parameterIndex)));
                }
            }

            return string.Join(", ", formattedArguments);
        }

        private static string FormatForLog(object obj)
        {
            if (obj == null)
            {
                return "<NULL>";
            }

            if (obj is string)
            {
                return string.Format(@"""{0}""", FormatString(obj.ToString()));
            }

            if (obj is char)
            {
                return string.Format("'{0}'", obj);
            }

            if (obj is Type)
            {
                return string.Format("Type[{0}]", ((Type)obj).Name);
            }

            var type = obj.GetType();
            if (obj is Exception)
            {
                return GetFriendlyName(type);
            }

            if (obj is MethodInfo)
            {
                return string.Format("MethodInfo[{0}.{1}]", GetFriendlyName(((MethodInfo)obj).DeclaringType), ((MethodInfo)obj).Name);
            }

            var toString = type.GetMethod("ToString", new Type[0]);
            if (type.IsValueType || (toString != null && toString.DeclaringType != typeof(object)))
            {
                return FormatString(obj.ToString());
            }

            var friendlyName = GetFriendlyName(type);
            if (obj is ICollection)
            {
                return string.Format("{0}(Count={1})", friendlyName, ((ICollection)obj).Count);
            }

            return friendlyName;
        }

        private static string GetFriendlyName(Type type)
        {
            Contract.Requires(type != null);

            //the namespace is null if it's an anonymous type
            if (type.IsGenericType)
            {
                return string.Format(
                    "{0}<{1}>",
                    type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.Ordinal)),
                    type.GetGenericArguments().Select(GetFriendlyName).Aggregate(
                        (current, friendlyName) => current + ", " + friendlyName));
            }

            if (type.Namespace == null)
            {
                return "AnonymousType";
            }

            return type.Name;
        }

        private static string FormatString(string source)
        {
            Contract.Requires(source != null);

            const int maxLength = 150;
            const string shortenWith = "... <snip> ...";
            // 14 is length of shortenWith
            const int indexHalfway = (maxLength / 2) - 14;

            if (source.Length < maxLength)
            {
                return source;
            }

            return string.Format("{0}{1}{2}", source.Substring(0, indexHalfway), shortenWith, source.Substring(source.Length - indexHalfway - 1));
        }
    }
}
