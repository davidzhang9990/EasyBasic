using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace XDDEasy.Contract
{
    [JsonObject]
    [DataContract]
    public class ResponseBase
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
    }
}
