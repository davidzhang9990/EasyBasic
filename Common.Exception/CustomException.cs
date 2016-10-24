using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common.Exception
{
    public class CustomException : System.Exception
    {
        private readonly HttpStatusCode _statusCode;
        public HttpStatusCode HttpStatusCode
        {
            get
            {
                return _statusCode;
            }
        }
        public CustomException(int code, string message)
            : base(JsonConvert.SerializeObject(new { Code = code, Message = message }))
        {

            _statusCode = HttpStatusCode.BadRequest;
        }

        public CustomException(HttpStatusCode statusCode, int code, string message)
            : base(JsonConvert.SerializeObject(new { Code = code, Message = message }))
        {
            _statusCode = statusCode;
        }

        public CustomException(int code, string message, System.Exception innerException)
            : base(JsonConvert.SerializeObject(new { Code = code, Message = message }), innerException)
        {
            _statusCode = HttpStatusCode.BadRequest;
        }
    }
}
