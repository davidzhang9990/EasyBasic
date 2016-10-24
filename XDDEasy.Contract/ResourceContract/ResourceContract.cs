using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XDDEasy.Contract.ResourceContract
{
    [JsonObject]
    public class ResourceRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Culture { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
    }

    public class ResourceResponse : ResponseBase
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Culture { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
    }
}
