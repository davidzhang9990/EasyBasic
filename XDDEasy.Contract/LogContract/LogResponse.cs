using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDDEasy.Contract.LogContract
{
    public class LogResponse : ResponseBase
    {
        public string Content { get; set; }
        public int Type { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public string Address { get; set; }
        public string Property { get; set; }
    }
}
