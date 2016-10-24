using System.IO;
using System.Runtime.Remoting.Messaging;
using log4net.Core;
using log4net.Layout.Pattern;

namespace Common.Log
{
    class UserIdPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            writer.Write(CallContext.LogicalGetData(LogProperties.UserId) ?? "");
        }
    }
}
