using System.Net;

namespace Common.Exception
{
    public class InternalServerErrorException : StatusException
    {
        public override HttpStatusCode HttpStatusCode
        {
            get { return HttpStatusCode.InternalServerError; }
        }

        public InternalServerErrorException(string message) : base(message) { }

        public InternalServerErrorException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
