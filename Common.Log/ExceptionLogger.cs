using System;
using System.Diagnostics.Contracts;
using System.Text;
using log4net;

namespace Common.Log
{
    public static class ExceptionLogger
    {
        public static void Exception(this ILog log, string message, Exception exception)
        {
            Contract.Requires(exception != null);

            var stringBuilder = new StringBuilder();
            if (message != null)
            {
                stringBuilder.AppendLine(message);
            }
            AddException(stringBuilder, exception);
            log.Error(stringBuilder);
        }

        public static void Exception(this ILog log, Exception exception)
        {
            Contract.Requires(exception != null);

            Exception(log, null, exception);
        }

        private static void AddException(StringBuilder stringBuilder, Exception exception)
        {
            Contract.Requires(stringBuilder != null);
            Contract.Requires(exception != null);

            stringBuilder
                .AppendFormat("Exception of type {0} with message {1}", exception.GetType(), exception.Message)
                .AppendLine()
                .AppendFormat("Stack Trace: {0}", exception.StackTrace)
                .AppendLine();

            if (exception.InnerException != null)
            {
                AddException(stringBuilder, exception.InnerException);
            }
        }
    }
}
