using NhatH.MVC.CarInventory.Core.Core.Helper;
using NhatH.MVC.CarInventory.Core.Framework.DatabaseConfiguration;
using NhatH.MVC.CarInventory.Core.Framework.IoC;
using System;
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
            try
            {
                var installDatabase = new InstallDatabase(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                installDatabase.InitializerDatabase();
                installDatabase.UpdateDatabase();
            }
            catch (Exception exc)
            {
                exc.Error("An error was occurred on application.");
                Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
            }

            BundleTable.EnableOptimizations = true;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyResolver.SetResolver(AutofacIocAdapter.Instance.GetResolver());
        }

        void Application_Error(Object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError().GetBaseException();
            exc.Error("An error was occurred on application.");
            Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
        }
    }
}
