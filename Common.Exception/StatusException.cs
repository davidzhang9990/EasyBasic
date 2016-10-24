using System;

namespace Common.Exception
{
    public abstract class StatusException : System.Exception
    {
        protected StatusException()
        {
        }

        protected StatusException(string message)
            : base(message)
        {
        }

        protected StatusException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public abstract System.Net.HttpStatusCode HttpStatusCode { get; }
    }
}
