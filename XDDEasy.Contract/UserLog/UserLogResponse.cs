using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace XDDEasy.Contract.UserLog
{
    public class UserLogResponse : ResponseBase
    {
        public UserLogResponse(RequestContext context = null)
            : base()
        {
            if (context != null)
            {
                this.UserId = context.UserId.ToString();
            }
        }
        public string UserId { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public string UserAgent { get; set; }
        public string Address { get; set; }
        public string Ip { get; set; }
        public string Host { get; set; }
    }
}
