using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace Common.Models
{
    [Serializable]
    public class RequestContext : MarshalByRefObject// Need this for now due to this:  http://msdn.microsoft.com/en-us/library/dn458353(v=vs.110).aspx and this http://stackoverflow.com/questions/15693262/serialization-exception-in-net-4-5
    {
        public const string Key = "Easy.RequestContext";
        
        public Guid UserId { get; set; }
        public List<EnumRole> Roles { get; set; }
        public List<string> RoleIds { get; set; }
        //public string HostAdress { get; set; }
        public string UserName { get; set; }

        public static void SetToCallContext(RequestContext requestContext)
        {
            CallContext.LogicalSetData(Key, requestContext);
        }

        public static RequestContext GetFromCallContext()
        {
            try
            {
                if (CallContext.LogicalGetData(Key) == null)
                    return null;
                else
                    return CallContext.LogicalGetData(Key) as RequestContext;
            }
            catch
            {
                return null;
            }
        }
    }
}
