using System.Data.Entity;
using XDDEasy.Domain.AccountAggregates;
using Microsoft.AspNet.Identity.EntityFramework;

namespace XDDEasy.Domain.Identity
{
    public class EasyUserStore : UserStore<User>
    {
        public EasyUserStore(DbContext context)
            : base(context)
        {
        }
    }
}
