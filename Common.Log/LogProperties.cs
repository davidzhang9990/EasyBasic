using System.Runtime.Remoting.Messaging;

namespace Common.Log
{
    public class LogProperties : ILogProperties
    {
        public const string UserId = "UserId";

        public void SetUserIdPropertyPerThread(string value)
        {
            SetThreadProperty(UserId, value);
        }

        private static void SetThreadProperty(string name, string value)
        {
            CallContext.LogicalSetData(name, value);
        }
    }

    public interface ILogProperties
    {
        void SetUserIdPropertyPerThread(string value);
    }
}
