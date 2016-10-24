using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDDEasy.Contract.EmailContract
{
    public class
        EmailResponse : ResponseBase
    {
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailTitle { get; set; }
        public string EmailBody { get; set; }
    }
}
