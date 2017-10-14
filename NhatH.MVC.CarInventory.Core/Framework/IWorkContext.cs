using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;
using NhatH.MVC.CarInventory.Core.Service.Contract;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Web;

namespace NhatH.MVC.CarInventory.Core.Framework
{
    public interface IWorkContext
    {
        UserProfileModel CurrentUserProfile { get; set; }
        RoleModel Role { get; set; }
        bool IsAdministrator { get; }
    }

    public interface IWebWorkContext { }

    public interface IAppWorkContext { }

    public class AppWorkContext : IWorkContext, IAppWorkContext
    {
        private const string cookieName = "CarInventory.TokenCookie.Guest";
        private UserProfileModel _userProfile;


        private RoleModel _role;

        public AppWorkContext()
        {
            this._userProfile = new UserProfileModel
            {
                UserId = 0,
                UserName = "Administrator"
            };
        }

        public UserProfileModel CurrentUserProfile
        {
            get { return _userProfile; }
            set { this._userProfile = value; }
        }


        public RoleModel Role
        {
            get { return _role; }
            set { this._role = value; }
        }

        public bool IsAdministrator
        {
            get
            {
                return true;
            }
        }
    }

    public class WebWorkContext : IWorkContext, IWebWorkContext
    {
        private readonly HttpContextBase _httpContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMemberService _memberService;
        private const string cookieName = "CarInventory.TokenCookie.Guest";
        private UserProfileModel _cachedUserProfile;
        private RoleModel _cachedRole;

        public WebWorkContext(HttpContextBase httpContext, 
                           IAuthenticationService authenticationService,
                           IMemberService memberService)
        {
            _httpContext = httpContext;
            _authenticationService = authenticationService;
            _memberService = memberService;
        }

        public virtual UserProfileModel CurrentUserProfile
        {
            get
            {
                if (_cachedUserProfile != null)
                    return _cachedUserProfile;

                UserProfile authenticatedUserProfile = _authenticationService.GetAuthenticatedUserProfile();
                if (authenticatedUserProfile != null)
                {
                    if (string.IsNullOrEmpty(authenticatedUserProfile.DateFormat))
                        authenticatedUserProfile.DecimalSymbol =  ",";
                    if (string.IsNullOrEmpty(authenticatedUserProfile.ThousandSymbol))
                        authenticatedUserProfile.ThousandSymbol = ".";
                    if (string.IsNullOrEmpty(authenticatedUserProfile.DateFormat))
                        authenticatedUserProfile.DateFormat = "dd-mm-yyyy";

                }

                _cachedUserProfile = new UserProfileModel().ToModel(authenticatedUserProfile);

                return _cachedUserProfile;
            }
            set
            {
                SetCustomerCookie(_cachedUserProfile.UserGuid);
                _cachedUserProfile = value;
            }
        }

 
        protected HttpCookie GetCustomerCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            HttpCookieCollection httpCookieCollection = _httpContext.Request.Cookies;
            return httpCookieCollection[cookieName];
        }

        protected void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                
                var cookie = new HttpCookie(cookieName);
                cookie.HttpOnly = true;
                cookie.Value = customerGuid.ToString();
                if (customerGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365;
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(cookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public virtual RoleModel Role
        {
            get
            {
                if (_cachedUserProfile == null || _cachedUserProfile.UserName == null)
                    return null;
                if (_cachedRole != null)
                    return _cachedRole;

                var role = _memberService.GetRoleByUserName(_cachedUserProfile.UserName);
                if (role == null) return null;
                _cachedRole = role;
                return _cachedRole;
            }
            set { _cachedRole = value; }
        }

        public bool IsAdministrator
        {
            get
            {
                return this.Role.IsSuperUser;
            }
        }
    }
}
