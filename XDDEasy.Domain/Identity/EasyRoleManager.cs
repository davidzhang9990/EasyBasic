using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDDEasy.Domain.Identity
{
    public class EasyRoleManager : RoleManager<IdentityRole>
    {
        public EasyRoleManager(EasyRoleStore store)
            : base(store)
        {
        }

        //public static EqlRoleManager Create(IdentityFactoryOptions<EqlUserManager> options, IOwinContext context)
        //{
        //    var manager = new EqlRoleManager(new EqlRoleStore(context.Get<ExamContext>()));
        //    return manager;
        //}
    }
}
