using NhatH.MVC.CarInventory.Core.Service.Contract;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Web;
using System.Web.Security;

namespace NhatH.MVC.CarInventory.Core.Service.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        private readonly IMemberService _memberService;
        //private readonly IFunctionService _functionService;

        private UserProfile _cachedUserProfile;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="memberService">The member service.</param>
        /// <param name="functionService">The function service.</param>
        public AuthenticationService(HttpContextBase httpContext, IMemberService memberService)//, IFunctionService functionService)
        {
            _httpContext = httpContext;
            _memberService = memberService;
            //_functionService = functionService;
        }

        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="persistCookie">if set to <c>true</c> [persist cookie].</param>
        /// <returns></returns>
        public virtual bool SignIn(string userName, string password, bool persistCookie = false)
        {
            bool userLogin = _memberService.Login(userName, password, persistCookie);

            if (userLogin)
            {
                CachingUserProfile(userName);
            }

            return userLogin;
        }

        /// <summary>
        /// Cachings the user profile.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        private void CachingUserProfile(string userName)
        {
            var insertUserProfileIfN = _memberService.GetUserProfile(userName);
            _cachedUserProfile = insertUserProfileIfN;
        }

        /// <summary>
        /// Signs the out.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public virtual void SignOut(string userName)
        {
            _cachedUserProfile = null;
            //_functionService.ClearCacheFunction(userName);
            _memberService.Logout();
        }

        /// <summary>
        /// Gets the authenticated user profile.
        /// </summary>
        /// <returns></returns>
        public virtual UserProfile GetAuthenticatedUserProfile()
        {
            if (_cachedUserProfile != null)
                return _cachedUserProfile;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            UserProfile authenticatedUserProfile = GetAuthenticatedUserProfile(formsIdentity.Ticket);
            if (authenticatedUserProfile != null)
                _cachedUserProfile = authenticatedUserProfile;
            return authenticatedUserProfile;
        }

        /// <summary>
        /// Gets the authenticated user profile.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">ticket</exception>
        public virtual UserProfile GetAuthenticatedUserProfile(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            string userName = ticket.Name; // email address.

            return String.IsNullOrWhiteSpace(userName) ? null : _memberService.GetUserProfile(userName);
        }

        /// <summary>
        /// Is user already logged in 
        /// </summary>
        /// <returns></returns>
        public bool IsLoggedin()
        {
            return _memberService.IsAuthenticated();
        }


    }
}
