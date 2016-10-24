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
        Student = 0,
        Teacher = 1,
        System = 2,
        SchoolAdmin = 3,
        RegisteredTeacher = 4,
        Parent = 5,
        SchoolPublicAccount = 40,
        ChannelAccount = 50
    }

    [DataContract]
    public enum EnumScope
    {
        System = 0,
        School = 1,
        Personal = 2
    }
}
