using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;
using NhatH.MVC.CarInventory.Core.Framework;
using NhatH.MVC.CarInventory.Core.Service.Contract;
using NhatH.MVC.CarInventory.Web.App_Start;
using NhatH.MVC.CarInventory.Web.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NhatH.MVC.CarInventory.Web.Controllers
{
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMemberService _memberService;
        private readonly IWorkContext _workContext;

        public AccountController(IMemberService memberService, IAuthenticationService authenticationService,
                                 IWorkContext workContext, IAuthorizationService authorizationService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _memberService = memberService;
            _workContext = workContext;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (_authenticationService.IsLoggedin())
            {
                return (ActionResult)RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (_authenticationService.SignIn(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return string.IsNullOrEmpty(returnUrl) ? (ActionResult)RedirectToAction("Index", "Home") : Redirect(returnUrl);
            }
            return View(model);
        }



        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _memberService.GetUser(model.UserName);
                if (user != null)
                {
                    AddErrors(new IdentityResult("Duplicate user name. Please try another."));
                }
                else
                {
                    var role = _memberService.GetRoleByRole("System Manager");
                    if (role == null)
                    {
                        AddErrors(new IdentityResult("Role Not Found."));
                    }
                    else
                    {
                        var userModel = new UserModel()
                        {
                            UserName = model.UserName,
                            PassWord = model.Password,
                            UserProfiles = new Collection<UserProfileModel>(new List<UserProfileModel>(){new UserProfileModel()
                        {
                            Email = model.Email,
                            IsActived = true,
                            Name = model.UserName,
                            IsNotDeletable = false
                        }}),

                            Roles = new Collection<RoleModel>(new List<RoleModel>() { new RoleModel() { Id = role.Id } })
                        };

                        var createUserState = _memberService.CreateUser(userModel);
                        if (createUserState == Core.Core.Model.Type.ResultType.Success)
                        {
                            if (_authenticationService.SignIn(model.UserName, model.Password, persistCookie: true))
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                AddErrors(new IdentityResult("An Error Occurred. please contact your administrator."));
                            }
                        }
                        else
                        {
                            AddErrors(new IdentityResult("An Error Occurred. please contact your administrator."));
                        }
                    }

                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }


        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }



        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _authenticationService.SignOut(User.Identity.Name);
            HttpContext.Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}