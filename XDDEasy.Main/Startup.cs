using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(XDDEasy.Main.Startup))]
namespace XDDEasy.Main
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            Domain.Startup.ConfigureAuth(app);
        }
    }
}
