using NhatH.MVC.CarInventory.Core.Service.Contract;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NhatH.MVC.CarInventory.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CarInventoryAuthorizeAttribute: AuthorizeAttribute
    {
        private string[] permissions { get; set; }

        #region Public Properties

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        public PermissionOperator Operator { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        public string[] Permissions
        {
            get
            {
                return this.permissions;
            }
            set
            {
                this.permissions = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The on authorization.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                                     || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(
                                         typeof(AllowAnonymousAttribute),
                                         true);
            if (skipAuthorization)
            {
                return;
            }

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // auth failed, redirect to login page
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else if (this.AuthorizeCore(filterContext.HttpContext))
            {
                this.SetCachePolicy(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The authorize core.
        /// </summary>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            var authorizationService = DependencyResolver.Current.GetService<IAuthorizationService>();
            if (!string.IsNullOrEmpty(this.Permission))
            {
                return authorizationService.HasPermission(this.Permission);
            }

            if (this.Permissions != null && this.Permissions.Any())
            {
                switch (this.Operator)
                {
                    case PermissionOperator.And:
                        {
                            bool flag = true;
                            this.Permissions.ToList().ForEach(c => { flag = flag && authorizationService.HasPermission(c); });

                            return flag;
                        }

                        break;
                    case PermissionOperator.Or:
                        {
                            bool flag = true;
                            this.Permissions.ToList().ForEach(
                                c =>
                                {
                                    flag = authorizationService.HasPermission(c);
                                    if (flag)
                                    {
                                    }
                                });

                            return flag;
                        }

                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// The cache validate handler.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="validationStatus">
        /// The validation status.
        /// </param>
        protected void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization(new HttpContextWrapper(context));
        }

        /// <summary>
        /// The handle unauthorized request.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result =
                   new RedirectToRouteResult(
                       new RouteValueDictionary(new { controller = "Error", action = "Unauthorised" }));
        }

        /// <summary>
        /// The set cache policy.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        protected void SetCachePolicy(AuthorizationContext filterContext)
        {
            // ** IMPORTANT **
            // Since we're performing authorization at the action level, the authorization code runs
            // after the output caching module. In the worst case this could allow an authorized user
            // to cause the page to be cached, then an unauthorized user would later be served the
            // cached page. We work around this by telling proxies not to cache the sensitive page,
            // then we hook our custom authorization code into the caching mechanism so that we have
            // the final say on whether a page should be served from the cache.
            HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
            cachePolicy.SetProxyMaxAge(new TimeSpan(0));
            cachePolicy.AddValidationCallback(this.CacheValidateHandler, null /* data */);
        }

        #endregion
    }
    public enum PermissionOperator
    {
        /// <summary>
        /// The and.
        /// </summary>
        And,

        /// <summary>
        /// The or.
        /// </summary>
        Or
    }
}