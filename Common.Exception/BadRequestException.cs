using System.Net;

namespace Common.Exception
{
    public class BadRequestException : StatusException
    {
        public override HttpStatusCode HttpStatusCode
        {
            get { return HttpStatusCode.BadRequest; }
        }

        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
