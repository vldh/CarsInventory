using FluentValidation.Attributes;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator;
using System.ComponentModel;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential
{
    [Validator(typeof(ChangePasswordValidator))]
    public class ChangePasswordModel : BaseModel
    {
        [DisplayName("ChangePassword.UserName")]
        public string UserName { get; set; }

        [DisplayName("ChangePassword.CurrentPassWord")]
        public string CurrentPassWord { get; set; }

        [DisplayName("ChangePassword.NewPassWord")]
        public string NewPassWord { get; set; }

        [DisplayName("ChangePassword.ConfirmPassWord")]
        public string ConfirmPassWord { get; set; }
    }
}
