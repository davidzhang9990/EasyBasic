using System.Runtime.Serialization;

namespace Common.Models.Enum
{
    [DataContract]
    public enum EmailType
    {
        [DataMember]
        RegistrationEmail = 1,
        [DataMember]
        ResetPassEmail = 2
    }
}
