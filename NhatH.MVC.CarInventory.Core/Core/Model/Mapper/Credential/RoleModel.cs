using AutoMapper;
using FluentValidation.Attributes;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Function;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential
{
    [Validator(typeof(RoleModelValidator))]
    public class RoleModel : BaseModel
    {
        [DisplayName("RoleModel.RoleName")]
        public string RoleName { get; set; }
        [DisplayName("RoleModel.IsActived")]
        public bool IsActived { get; set; }
        [DisplayName("RoleModel.IsNotDeletable")]
        public bool IsNotDeletable { get; set; }
        [DisplayName("RoleModel.Description")]
        public string Description { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public bool IsSuperUser { get { return Id == 1 ? true : false; } }
        private ICollection<RoleFunctionModel> _roleFunctions;
        public virtual ICollection<RoleFunctionModel> RoleFunctions
        {
            get { return _roleFunctions ?? (_roleFunctions = new List<RoleFunctionModel>()); }
            set { _roleFunctions = value; }
        }

        /*private ICollection<UserModel> _users;
        public virtual ICollection<UserModel> Users
        {
            get { return _users ?? (_users = new List<UserModel>()); }
            set { _users = value; }
        }*/

        public RoleModel ToModel(Role entity)
        {
            
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Role, RoleModel>().ForMember(x => x.RoleFunctions, opt => opt.MapFrom(src => src.RoleFunctions));
                cfg.CreateMap<RoleFunction, RoleFunctionModel>();
            });
            IMapper mapper = config.CreateMapper();

            return mapper.Map<Role, RoleModel>(entity);
        }

        public Role ToEntity()
        {
            
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RoleModel, Role>().ForMember(x => x.RoleFunctions, expression => expression.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<RoleModel, Role>(this);
        }

        public Role ToEntity(Role entity)
        {
            
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RoleModel, Role>().ForMember(x => x.ID, expression => expression.Ignore())
                .ForMember(x => x.RoleFunctions, expression => expression.Ignore());
            });
            IMapper mapper = config.CreateMapper();

            return mapper.Map(this, entity);
        }

        public Role ToEntityForEdit()
        {
            
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RoleModel, Role>().ForMember(x => x.ID, opt => opt.MapFrom(src => src.Id))
                .ForAllMembers(w => w.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<RoleModel, Role>(this);
        }
    }
}
