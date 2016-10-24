using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDDEasy.Domain.Identity
{
    public class EasyRoleStore : RoleStore<IdentityRole>
    {
        public EasyRoleStore(DbContext context)
            : base(context)
        {
        }
    }
}
