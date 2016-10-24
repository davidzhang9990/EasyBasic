using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    [DataContract]
    public class IconInfo
    {
        [DataMember]
        public Uri IconUrl { get; set; }

        [DataMember]
        public Guid Id { get; set; }

    }
}
