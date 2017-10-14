using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;
using NhatH.MVC.CarInventory.Core.Core.Model.Type;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NhatH.MVC.CarInventory.Core.Service.Contract
{
    public interface IMemberService : ICarInventoryService
    {
        bool ChangePassword(ChangePasswordModel changePasswordModel);
        bool ChangeAvatar(int userId, string avatarPath);
        ResultType CreateRole(RoleModel roleModel);
        ResultType CreateUser(UserModel userModel);
        bool DeleteRole(int roleId);
        bool DeleteUser(string userName);
        string GeneratePasswordResetToken(string username);
        System.Collections.Generic.List<RoleModel> GetAllRoles();
        UserProfile GetGuestUserProfile(Guid guid);
        RoleModel GetRoleById(int roleId);
        RoleModel GetRoleByRole(string roleName);
        System.Linq.IQueryable<Domain.Role> GetRoles();
        Domain.User GetUser(string userName);
        UserModel GetUserByUserName(string userName);
        int GetUserId(string userName);
        Domain.UserProfile GetUserProfile(string userName);
        Domain.UserProfile GetUserProfileById(int id);
        System.Linq.IQueryable<Domain.User> GetUsers();
        List<UserModel> GetAllUsers();
        System.Linq.IQueryable<Domain.User> GetUsersInRole(int roleId);
        bool InsertedUser(UserModel model);
        Domain.UserProfile InsertGuestUserProfile();
        Domain.UserProfile InsertUserProfileIfN(UserModel userModel);
        bool IsExistedUser(string userName);
        bool IsSystemAdmin(string userName);
        bool Login(string userName, string password, bool persistCookie = false);
        void Logout();
        bool ResetPassword(string email, string newpassword, string tokenPassword, string subject, string body);
        void SendNotifyResetPasswordEmail(string email, string subject, string body);
        void SendRequestResetPasswordEmail(string email, string link, string subject, string body);
        Domain.UserProfile UpdateGuestUserProfile(Domain.UserProfile userProfile);
        ResultType UpdateRole(RoleModel roleModel);
        ResultType UpdateUser(UserModel userModel);
        RoleModel GetRoleByUserName(string userName);
        bool IsAuthenticated();
        User GetUser(int id);
        //CultureInfo GetUserCultureInfo(int userId);
        DateTime GetAllowedBatchAssessmentDate(int userId);
        IQueryable<UserProfile> GetUserProfiles();
        bool DeleteAllNormalRoles();
        bool DeleteAllNormalUsers();
    }
}
