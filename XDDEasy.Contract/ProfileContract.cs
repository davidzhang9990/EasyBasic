using System;
using System.Runtime.Serialization;

namespace XDDEasy.Contract
{
    public class ProfileResponse : ResponseBase
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    public class ProfileRequest
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public Guid? SchoolId { get; set; }
    }

    public enum EnumProfileName
    {
        IPadVersion,
        IsUsingSimpleLogin,
        ShareFaceBook,
        RestrictedSessionCount,
        [DataMember]
        SmsService
    }
}
