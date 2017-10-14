using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;
using NhatH.MVC.CarInventory.Core.Framework.IoC;
using NhatH.MVC.CarInventory.Core.Service.Contract;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace NhatH.MVC.CarInventory.Web.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                try
                {
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "User", "Id", "UserName", autoCreateTables: true);

                    CreateRole();
                    CreateUser();

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }

            private void CreateUser()
            {
                var memberService = (IMemberService)AutofacIocAdapter.Instance.GetResolver().GetService(typeof(IMemberService));
                var user = memberService.GetUser("Admin"); if (user != null) return;
                var role = memberService.GetRoleByRole("System Manager"); if (role == null) return;
                var userModel = new UserModel()
                {
                    UserName = "Admin",
                    PassWord = "admin",
                    UserProfiles = new Collection<UserProfileModel>(new List<UserProfileModel>(){new UserProfileModel()
                        {
                            Email = "Admin@admin.com",
                            IsActived = true,
                            Name = "Admin",
                            IsNotDeletable = true
                        }}),

                    Roles = new Collection<RoleModel>(new List<RoleModel>() { new RoleModel() { Id = role.Id } })
                };

                memberService.CreateUser(userModel);
            }

            private void CreateRole()
            {
                var memberService =
                       (IMemberService)AutofacIocAdapter.Instance.GetResolver().GetService(typeof(IMemberService));
                var role = memberService.GetRoleByRole("System Manager");
                if (role == null)
                {
                    var rolemodel = new RoleModel()
                    {
                        RoleName = "System Manager",
                        Description = "System Manager",
                        IsActived = true,
                    };

                    memberService.CreateRole(rolemodel);
                }
            }
        }
    }
}