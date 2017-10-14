using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NhatH.MVC.CarInventory.Web.Startup))]
namespace NhatH.MVC.CarInventory.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
