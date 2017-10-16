using FluentValidation.Attributes;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential
{
    [Validator(typeof(LoginModelValidator))]
    public class LoginModel
    {
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }

        public string Message { get; set; }
    }
}
