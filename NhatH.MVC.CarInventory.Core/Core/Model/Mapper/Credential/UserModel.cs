using AutoMapper;
using FluentValidation.Attributes;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential
{
    [Validator(typeof(UserModelValidator))]
    public class UserModel : BaseModel
    {
        [DisplayName("UserModel.UserName")]
        public string UserName { get; set; }

        [DisplayName("UserModel.PassWord")]
        public string PassWord { get; set; }

        [DisplayName("ChangePassword.ConfirmPassWord")]
        public string ConfirmPassWord { get; set; }

        public bool IsDelete { get; set; }

        private ICollection<UserProfileModel> _userProfiles;

        public virtual ICollection<UserProfileModel> UserProfiles
        {
            get { return _userProfiles ?? (_userProfiles = new List<UserProfileModel>()); }
            set { _userProfiles = value; }
        }

        private ICollection<RoleModel> _roles;

        public virtual ICollection<RoleModel> Roles
        {
            get { return _roles ?? (_roles = new List<RoleModel>()); }
            set { _roles = value; }
        }

        public UserModel ToModel(User entity)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserModel>();
                cfg.CreateMap<Role, RoleModel>().ForMember(w => w.RoleFunctions, exp => exp.Ignore());
                cfg.CreateMap<UserProfile, UserProfileModel>();
            });
            IMapper mapper = config.CreateMapper();


            return mapper.Map<User, UserModel>(entity);
        }

        public User ToEntity()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserModel, User>()
                .ForMember(x => x.ID, expression => expression.Ignore())
                .ForMember(x => x.Roles, expression => expression.Ignore())
                ;
                cfg.CreateMap<UserProfileModel, UserProfile>()
                .ForMember(x => x.ID, expression => expression.Ignore());
            });
            IMapper mapper = config.CreateMapper();

            return mapper.Map<UserModel, User>(this);
        }

        public User ToEntity(UserModel model, User entity)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserModel, User>()
                .ForMember(x => x.ID, expression => expression.Ignore())
                .ForMember(x => x.Roles, expression => expression.Ignore())
                .ForMember(x => x.UserProfiles, expression => expression.Ignore())
                ;
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map(model, entity);
        }
    }
}
