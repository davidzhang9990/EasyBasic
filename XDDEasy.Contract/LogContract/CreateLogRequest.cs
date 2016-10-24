using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XDDEasy.Contract.LogContract
{
    [JsonObject]
    [DataContract]
    public class CreateLogRequest
    {
        [DataMember(IsRequired = true)]
        public string Content { get; set; }
        [DataMember]
        public int Type { get; set; }
        [DataMember]
        public string Property { get; set; }
    }
}
