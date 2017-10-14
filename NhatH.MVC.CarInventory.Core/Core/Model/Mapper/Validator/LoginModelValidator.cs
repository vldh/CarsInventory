using FluentValidation;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(l => l.UserName).NotEmpty().WithMessage("LoginModelValidtor.RequireUsername");
            RuleFor(l => l.Password).NotEmpty().WithMessage("LoginModelValidtor.RequirePassword");
        }
    }
}
