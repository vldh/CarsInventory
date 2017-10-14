using NhatH.MVC.CarInventory.Core.Framework.DatabaseConfiguration;
using NhatH.MVC.CarInventory.Core.Framework.IoC;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NhatH.MVC.CarInventory.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var installDatabase =
              new InstallDatabase(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            //installDatabase.CreateDatabase();
            installDatabase.InitializerDatabase();
            installDatabase.UpdateDatabase();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyResolver.SetResolver(AutofacIocAdapter.Instance.GetResolver());
        }
    }
}
