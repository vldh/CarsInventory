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
        private readonly IWorkContext _workContext;

        public AuthorizationService(IMemberService memberService, IWorkContext workContext)
        {
            _memberService = memberService;
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
            //only check function that is in the list that administrator can grant access to
            if (IsAnonymous()) return false;

            return true;
        }


    }
}
