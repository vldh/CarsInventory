using AutoMapper;
using FluentValidation.Attributes;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.ComponentModel;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential
{
    [Validator(typeof(UserProfileValidator))]
    public class UserProfileModel : BaseModel
    {
        public int Idx { get; set; }
        public Guid UserGuid { get; set; }
        public int? UserId { get; set; }
        [DisplayName("UserProfile.UserName")]
        public string UserName { get; set; }
        public string Token { get; set; }

        [DisplayName("UserProfile.Email")]
        public string Email { get; set; }
        [DisplayName("UserProfile.Telephone")]
        public string Telephone { get; set; }
        [DisplayName("UserProfile.Mobile")]
        public string Mobile { get; set; }
        [DisplayName("UserProfile.Avatar")]
        public string Avatar { get; set; }
        [DisplayName("UserProfile.IsActived")]
        public bool IsActived { get; set; }
        public DateTime? ModifyDate { get; set; }
        [DisplayName("UserProfile.ModifiedBy")]
        public string ModifyBy { get; set; }
        public bool? IsNotDeletable { get; set; }
        [DisplayName("UserProfile.CreatedDate")]
        public DateTime? CreateDate { get; set; }
        public DateTime? LastChangePassword { get; set; }
        [DisplayName("ImportDataSettingModel.DecimalSymbol")]
        public string DecimalSymbol { get; set; }
        [DisplayName("ImportDataSettingModel.ThousandSymbol")]
        public string ThousandSymbol { get; set; }
        [DisplayName("ImportDataSettingModel.DateFormat")]
        public string DateFormat { get; set; }

        
        public UserProfileModel ToModel(UserProfile entity)
        {
            var config = new MapperConfiguration(cfg=> {
                cfg.CreateMap<UserProfile, UserProfileModel>().ForMember(w => w.Id, exp => exp.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            

            return mapper.Map<UserProfile, UserProfileModel>(entity);
        }

        public UserProfile ToEntity()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserProfileModel,UserProfile>().ForMember(w => w.ID, exp => exp.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            

            return mapper.Map<UserProfileModel, UserProfile>(this);
        }

        public UserProfile ToEntity(UserProfileModel model, UserProfile entity)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserProfile, UserProfileModel>().ForMember(x => x.Id, expression => expression.Ignore())
                 .ForMember(x => x.CreateDate, expression => expression.Ignore());
            });
            IMapper mapper = config.CreateMapper();

            
            return mapper.Map(model, entity);
        }
    }
}
