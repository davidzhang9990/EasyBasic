using log4net;

namespace Common.Log
{
    public class Log4NetTraceListener : System.Diagnostics.TraceListener
    {
        private readonly ILog _log;

        public Log4NetTraceListener()
        {
            _log = LogManager.GetLogger(GetType());
        }

        public Log4NetTraceListener(ILog log)
        {
            _log = log;
        }

        public override void Write(string message)
        {
            if (_log != null)
            {
                _log.Debug(message);
            }
        }

        public override void WriteLine(string message)
        {
            if (_log != null)
            {
                _log.Debug(message);
            }
        }
    }
}
