using NhatH.MVC.CarInventory.Core.Framework;
using NhatH.MVC.CarInventory.Core.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NhatH.MVC.CarInventory.Core.Service.Impl
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IMemberService _memberService;
        //private readonly IFunctionService _functionService;
        private readonly IWorkContext _workContext;

        public AuthorizationService(IMemberService memberService, IWorkContext workContext)
        {
            _memberService = memberService;
            //_functionService = functionService;
            _workContext = workContext;
        }

        public string[] CurrentPermissions
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["CURRENT_PERMISSIONS"] != null)
                    {
                        return HttpContext.Current.Session["CURRENT_PERMISSIONS"] as string[];
                    }
                }

                return null;
            }
            set
            {
                HttpContext.Current.Session["CURRENT_PERMISSIONS"] = value;
            }
        }


        public bool IsAnonymous()
        {
            return !System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated;
        }

        public bool IsSuperAdmin()
        {
            return "Admin".Equals(Thread.CurrentPrincipal.Identity.Name, StringComparison.InvariantCultureIgnoreCase);
        }
        public bool IsSuperAdmin(string user)
        {
            return "Admin".Equals(user, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified function has permission.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public bool HasPermission(string functionName)
        {
            //always return true if it is license active/ license request function and the user is admin
            //if (_functionService.RequireFunctions().Contains(functionName) && IsSuperAdmin())
            //    return true;

            //only check function that is in the list that administrator can grant access to
            var hasLicense = false;
            if (IsAnonymous()) return false;

            // if function does not exist, it is allowed for all registered users
            //var function = _functionService.GetFunction(functionName);
            ////if (function == null) return true;

            

            return true;
        }

        
    }
}
