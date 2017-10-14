using System.Web;
using System.Web.Mvc;

namespace NhatH.MVC.CarInventory.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
