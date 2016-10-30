using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XDDEasy.Contract
{
    public class PageResponse : ResponseBase
    {
        public string Name { get; set; }
        public string ActionName { get; set; }
        public string ControlName { get; set; }
        public string Area { get; set; }
        public string ShowPage { get; set; }
    }

    [DataContract]
    [JsonObject]
    public class UpdatePageRequest
    {
        public string Name { get; set; }
        public string ActionName { get; set; }
        public string ControlName { get; set; }
        public string Area { get; set; }
        public string ShowPage { get; set; }
    }

    public class CreatePageRequest : UpdatePageRequest
    {
    }


    [DataContract]
    [JsonObject]
    public class RolePageResponse
    {
        public string RoleId { get; set; }
        public Guid PageId { get; set; }
        public string DisplayName { get; set; }
        public string ActionName { get; set; }
        public string ControlName { get; set; }
        public string Area { get; set; }
        public string ShowPage { get; set; }
    }

    [DataContract]
    [JsonObject]
    public class CreateRoleRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    [DataContract]
    [JsonObject]
    public class CreateRolePageRequest
    {
        public Guid RoleId { get; set; }
        public Guid PageId { get; set; }
        public string DisplayName { get; set; }
        public short Sequence { get; set; }
    }
}
