using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public enum EnumRole
    {
        System = 0,
        SystemAdmin = 1,
        SystemMember = 2
    }
}
