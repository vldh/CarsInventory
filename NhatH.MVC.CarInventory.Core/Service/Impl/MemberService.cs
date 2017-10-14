using NhatH.MVC.CarInventory.Core.Core.Model.Extension;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;
using NhatH.MVC.CarInventory.Core.Core.Model.Type;
using NhatH.MVC.CarInventory.Core.Service.Contract;
using NhatH.MVC.CarInventory.DB.UoW;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebMatrix.WebData;

namespace NhatH.MVC.CarInventory.Core.Service.Impl
{
    public class MemberService : IMemberService
    {
        private readonly ICarInventoryUoW _carInventoryUow;
        //private readonly IEventPublisher _eventPublisher;

        public MemberService(ICarInventoryUoW vrsUow)//, IEventPublisher eventPublisher)
        {
            _carInventoryUow = vrsUow;
            //_eventPublisher = eventPublisher;
        }

        #region Local account.

        /// <summary>
        /// Logins the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="persistCookie">if set to <c>true</c> [persist cookie].</param>
        /// <returns></returns>
        public bool Login(string userName, string password, bool persistCookie = false)
        {
            var user = _carInventoryUow.User.FindFirstOfDefault(c => c.UserName.ToLower(Thread.CurrentThread.CurrentCulture) == userName.ToLower(Thread.CurrentThread.CurrentCulture) && c.UserProfiles.Any(p => p.IsActived));
            if (user != null)
            {
                return WebSecurity.Login(userName, password, persistCookie);
            }

            return false;
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        public void Logout()
        {
            WebSecurity.Logout();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return WebSecurity.IsAuthenticated;
        }
        #endregion

        #region User

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns></returns>
        public ResultType CreateUser(UserModel userModel)
        {
            userModel.Roles.RemoveIf(c => c.Id <= 0);
            if (InsertedUser(userModel))
            {
                return ResultType.Success;
            }
            return ResultType.Duplicate;
        }

        /// <summary>
        /// Gets the name of the user by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public UserModel GetUserByUserName(string userName)
        {
            return
                new UserModel().ToModel(_carInventoryUow.User.FindFirstOfDefault(w => w.UserName.ToLower() == userName.ToLower()));
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns></returns>
        public ResultType UpdateUser(UserModel userModel)
        {
            var userWithNewUserName = _carInventoryUow.User.FindFirstOfDefault(w => w.UserName.ToLower() == userModel.UserName.ToLower());
            if (userWithNewUserName != null && userWithNewUserName.ID != userModel.Id)
                return ResultType.Duplicate;
            var userProfileModel = userModel.UserProfiles.FirstOrDefault();
            if (userProfileModel == null)
                return ResultType.Error;

            var user = _carInventoryUow.User.GetById(userModel.Id);
            var userProfile = user.UserProfiles.FirstOrDefault();
            userProfile.UserName = userModel.UserName;
            userProfile.Name = userProfileModel.Name;
            userProfile.Mobile = userProfileModel.Mobile;
            userProfile.Email = userProfileModel.Email;
            userProfile.ModifyDate = DateTime.Now;
            userProfile.ModifyBy = HttpContext.Current.User.Identity.Name;
            userProfile.DateFormat = userProfileModel.DateFormat;
            userProfile.DecimalSymbol = userProfileModel.DecimalSymbol;
            userProfile.ThousandSymbol = userProfileModel.ThousandSymbol;
            userProfile.IsActived = userProfileModel.IsActived;

            if (userModel.PassWord != null && !userModel.PassWord.Equals("Aaaa!1111"))
            {
                var token = WebSecurity.GeneratePasswordResetToken(userModel.UserName);
                WebSecurity.ResetPassword(token, userModel.PassWord);
                userProfile.LastChangePassword = DateTime.Now;
            }

            user = userModel.ToEntity(userModel, user);
            var oldRoleId = user.Roles.Select(w => w.ID).FirstOrDefault();
            var newRuleId = userModel.Roles.Select(w => w.Id).FirstOrDefault();
            if (oldRoleId != newRuleId)
            {
                user.Roles.Remove(user.Roles.FirstOrDefault());
                user.Roles.Add(_carInventoryUow.Role.GetById(newRuleId));
                userProfile.RoleChangedDate = DateTime.Now;
            }

            user.UserProfiles = new Collection<UserProfile>() { userProfile };
            _carInventoryUow.User.Update(user);
            _carInventoryUow.Commit();
            return ResultType.Success;
        }

        /// <summary>
        /// Inserteds the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public bool InsertedUser(UserModel model)
        {
            if (!IsExistedUser(model.UserName))
            {
                WebSecurity.CreateUserAndAccount(model.UserName, model.PassWord);
                InsertUserProfileIfN(model);

                var user = _carInventoryUow.User.FindFirstOfDefault(w => w.UserName == model.UserName);
                var roles = new List<Role>();
                model.Roles.ToList().ForEach(k =>
                {
                    var role = _carInventoryUow.Role.GetById(k.Id);
                    roles.Add(role);
                });
                user.Roles = roles;
                _carInventoryUow.User.Update(user);
                _carInventoryUow.Commit();

                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is existed user] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool IsExistedUser(string userName)
        {
            return WebSecurity.UserExists(userName);
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public User GetUser(string userName)
        {
            return _carInventoryUow.User.FindFirstOfDefault(x => x.UserName.ToLower() == userName.ToLower());
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool DeleteUser(string userName)
        {
            var user = GetUser(userName);
            if (user == null) return false;
            //user.IsDelete = true;
            _carInventoryUow.User.Update(user);
            _carInventoryUow.Commit();
            return true;
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="changePasswordModel">The change password model.</param>
        /// <returns></returns>
        public bool ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var isSuccess = WebSecurity.ChangePassword(changePasswordModel.UserName, changePasswordModel.CurrentPassWord, changePasswordModel.NewPassWord);
            //save last change password
            if (isSuccess)
            {
                var user = _carInventoryUow.User.FindFirstOfDefault(w => w.UserName == changePasswordModel.UserName);
                var userProfile = user.UserProfiles.FirstOrDefault();
                if (userProfile == null) return false;
                userProfile.LastChangePassword = DateTime.Now;
                _carInventoryUow.User.Update(user);
                _carInventoryUow.Commit();
            }
            return isSuccess;

        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> GetUsers()
        {
            return _carInventoryUow.User.GetAll();//.Where(c => !c.IsDelete.HasValue || (c.IsDelete.HasValue && !c.IsDelete.Value));
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUsers()
        {
            return _carInventoryUow.User.GetAll().Where(w => w.UserProfiles.All(q => q.IsActived)).ToList().Select(w => new UserModel()
            {
                UserName = w.UserName,
                Id = w.ID
            }).ToList();
        }

        /// <summary>
        /// Determines whether [is system admin] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool IsSystemAdmin(string userName)
        {
            var user = GetUserByUserName(userName);
            return user.UserProfiles.Any(w => w.IsNotDeletable == true);
        }

        public bool DeleteAllNormalUsers()
        {
            var users = _carInventoryUow.User.Find(c => c.UserProfiles.Any(w => w.IsNotDeletable != true));
            foreach (var user in users)
            {
                //user.IsDelete = true;
                _carInventoryUow.User.Update(user);
            }

            _carInventoryUow.Commit();
            return true;
        }

        #endregion

        #region UserProfile

        /// <summary>
        /// Inserts the user profile if n.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns></returns>
        public UserProfile InsertUserProfileIfN(UserModel userModel)
        {
            User user = GetUser(userModel.UserName);
            UserProfile userProfile = null;
            if (user != null)
            {
                userProfile = GetUserProfile(userModel.UserName);
                if (userProfile == null)
                {
                    var userProfileModel = userModel.UserProfiles.FirstOrDefault();
                    if (userProfileModel != null)
                    {
                        userProfileModel.UserId = user.ID;
                        userProfileModel.UserGuid = Guid.NewGuid();
                        userProfileModel.UserName = user.UserName;
                        userProfileModel.ModifyBy = HttpContext.Current.User.Identity.Name;
                        userProfileModel.ModifyDate = DateTime.Now;
                        userProfileModel.CreateDate = DateTime.Now;
                        var entity = userProfileModel.ToEntity();
                        entity.LastChangePassword = DateTime.Now;
                        entity.RoleChangedDate = userModel.Roles.Any() ? DateTime.Now : new DateTime?();
                        _carInventoryUow.UserProfile.Insert(entity);
                        _carInventoryUow.Commit();
                    }
                }
            }
            return userProfile;
        }

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public UserProfile GetUserProfile(string userName)
        {
            User user = GetUser(userName);
            return user != null
                       ? _carInventoryUow.UserProfile.FindFirstOfDefault(u => u.UserId.HasValue && u.UserId.Value == user.ID)
                       : null;
        }

        /// <summary>
        /// GetUser Profile By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserProfile GetUserProfileById(int id)
        {
            UserProfile result = _carInventoryUow.UserProfile.Find(u => u.UserId == id).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Gets the guest user profile.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public UserProfile GetGuestUserProfile(Guid guid)
        {
            return _carInventoryUow.UserProfile.FindFirstOfDefault(x => x.UserGuid == guid);
        }

        /// <summary>
        /// Inserts the guest user profile.
        /// </summary>
        /// <returns></returns>
        public UserProfile InsertGuestUserProfile()
        {
            var userProfile = new UserProfile { UserGuid = Guid.NewGuid() };
            _carInventoryUow.UserProfile.Insert(userProfile);
            _carInventoryUow.Commit();

            return userProfile;
        }

        /// <summary>
        /// Updates the guest user profile.
        /// </summary>
        /// <param name="userProfile">The user profile.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Invalid user profile</exception>
        public UserProfile UpdateGuestUserProfile(UserProfile userProfile)
        {
            UserProfile firstOfDefault =
                _carInventoryUow.UserProfile.FindFirstOfDefault(x => x.UserGuid == userProfile.UserGuid);
            if (firstOfDefault == null)
                throw new ArgumentException("Invalid user profile");

            
            firstOfDefault.UserId = userProfile.UserId;
            firstOfDefault.Token = userProfile.Token;
            firstOfDefault.Email = userProfile.Email;
            _carInventoryUow.UserProfile.Update(firstOfDefault);
            _carInventoryUow.Commit();

            return firstOfDefault;
        }

        #endregion

        #region Role

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="roleModel">The role model.</param>
        /// <returns></returns>
        public ResultType CreateRole(RoleModel roleModel)
        {
            if (_carInventoryUow.Role.Find(w => w.RoleName.ToLower() == roleModel.RoleName).Any()) return ResultType.Duplicate;

            roleModel.ModifyBy = HttpContext.Current.User.Identity.Name;
            roleModel.ModifyDate = DateTime.Now;

            _carInventoryUow.Role.Insert(roleModel.ToEntity());
            _carInventoryUow.Commit();
            return ResultType.Success;
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public RoleModel GetRoleById(int roleId)
        {
            return new RoleModel().ToModel(_carInventoryUow.Role.GetById(roleId));
        }

        /// <summary>
        /// Gets the role by role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public RoleModel GetRoleByRole(string roleName)
        {
            var role = _carInventoryUow.Role.FindFirstOfDefault(w => w.RoleName == roleName);
            return role != null ? new RoleModel().ToModel(role) : null;
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleModel">The role model.</param>
        /// <returns></returns>
        public ResultType UpdateRole(RoleModel roleModel)
        {
            var roleWithNewRoleName = _carInventoryUow.Role.FindFirstOfDefault(w => w.RoleName.ToLower() == roleModel.RoleName);
            if (roleWithNewRoleName != null && roleWithNewRoleName.ID != roleModel.Id) return ResultType.Duplicate;

            roleModel.ModifyBy = HttpContext.Current.User.Identity.Name;
            roleModel.ModifyDate = DateTime.Now;

            var role = _carInventoryUow.Role.GetById(roleModel.Id);
            _carInventoryUow.Role.Update(roleModel.ToEntity(role));
            _carInventoryUow.Commit();
            return ResultType.Success;
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public bool DeleteRole(int roleId)
        {
            var role = _carInventoryUow.Role.FindFirstOfDefault(w => w.ID == roleId && w.IsNotDeletable != true);
            if (role == null) return false;
            if (role.Users.Any()) return false;
            _carInventoryUow.Role.Delete(role);
            _carInventoryUow.Commit();
            return true;
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Role> GetRoles()
        {
            return _carInventoryUow.Role.GetAll();//.Find(c => !c.IsDelete.HasValue || !c.IsDelete.Value);
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns></returns>
        public List<RoleModel> GetAllRoles()
        {
            var roles = _carInventoryUow.Role.GetAll().Where(w => w.IsActived).ToList().Select(w => new Role()
            {
                RoleName = w.RoleName,
                ID = w.ID
            });
            return roles.ToList().ToModel<Role, RoleModel>();
        }

        /// <summary>
        /// Gets the users in role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public IQueryable<User> GetUsersInRole(int roleId)
        {
            return _carInventoryUow.User.Find(w => w.Roles.Any(q => q.ID == roleId)
            //&& (!w.IsDelete.HasValue || !w.IsDelete.Value)
            && w.UserProfiles.Any(c => c.IsActived));
        }

        /// <summary>
        /// Gets the name of the role by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public RoleModel GetRoleByUserName(string userName)
        {
            var user = _carInventoryUow.User.FindFirstOfDefault(w => w.UserName.ToLower() == userName.ToLower());
            var role = user.Roles.FirstOrDefault();
            return role != null ? new RoleModel().ToModel(role) : null;
        }

        public bool DeleteAllNormalRoles()
        {
            var roles = _carInventoryUow.Role.Find(w => w.IsNotDeletable != true);
            foreach (var role in roles)
            {
                //role.IsDelete = true;
                _carInventoryUow.Role.Update(role);
            }

            _carInventoryUow.Commit();
            return true;
        }

        #endregion

        #region Reset password
        /// <summary>
        /// Sends the request reset password email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="link">The link.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public void SendRequestResetPasswordEmail(string email, string link, string subject, string body)
        {
            body = body.Replace("{{Email}}", email);//insert email reset
            body = body.Replace("{{Link}}", link);//insert link reset
            //_eventPublisher.SendEmail(email, subject, body);
        }

        /// <summary>
        /// Sends the notify reset password email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public void SendNotifyResetPasswordEmail(string email, string subject, string body)
        {
            /*  string subject = _localizationService.GetResource("Email.ChangePasswordSuccess.Subject");
              string body = _localizationService.GetResource("Email.ChangePasswordSuccess.Body");*/
            //_eventPublisher.SendEmail(email, subject, body);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="userName">The email.</param>
        /// <param name="newpassword">The newpassword.</param>
        /// <param name="tokenPassword">The token password.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public bool ResetPassword(string userName, string newpassword, string tokenPassword, string subject, string body)
        {
            bool isSuccess = WebSecurity.ResetPassword(tokenPassword, newpassword);

            //save last change password
            if (isSuccess)
            {
                var user = _carInventoryUow.User.FindFirstOfDefault(w => w.UserName == userName);
                var userProfile = user.UserProfiles.FirstOrDefault();
                if (userProfile == null) return false;
                userProfile.LastChangePassword = DateTime.Now;
                _carInventoryUow.User.Update(user);
                _carInventoryUow.Commit();
            }
            return isSuccess;
        }

        /// <summary>
        /// Generates the password reset token.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public string GeneratePasswordResetToken(string username)
        {
            return WebSecurity.GeneratePasswordResetToken(username);
        }

        #endregion

        public bool ChangeAvatar(int userId, string avatarPath)
        {
            try
            {
                UserProfile userProfile = _carInventoryUow.UserProfile.FindFirstOfDefault(x => x.UserId == userId);
                if (userProfile == null)
                {
                    throw new ArgumentException("Invalid user profile");
                }


                userProfile.Avatar = avatarPath;
                this._carInventoryUow.UserProfile.Update(userProfile);
                this._carInventoryUow.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User GetUser(int id)
        {
            return _carInventoryUow.User.FindFirstOfDefault(x => x.ID == id);
        }

        public DateTime GetAllowedBatchAssessmentDate(int userId)
        {
            var user = _carInventoryUow.User.FindFirstOfDefault(c => c.ID == userId);
            if (user == null)
            {
                throw new NotImplementedException();
            }

            var profile = user.UserProfiles.FirstOrDefault();
            if (profile == null)
            {
                throw new NotImplementedException();
            }

            var defaultDate = profile.CreateDate.HasValue ? profile.CreateDate.Value : DateTime.Now;
            return user.Roles.Any() && profile.RoleChangedDate.HasValue ? profile.RoleChangedDate.Value : defaultDate;
        }

        public IQueryable<UserProfile> GetUserProfiles()
        {
            return _carInventoryUow.UserProfile.Find(c => c.IsActived);
        }
    }
}
