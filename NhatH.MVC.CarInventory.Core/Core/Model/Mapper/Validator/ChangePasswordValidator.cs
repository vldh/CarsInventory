using FluentValidation;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(l => l.UserName).NotEmpty().WithMessage("ChangePasswordValidator.RequireUserName");
            RuleFor(l => l.CurrentPassWord).NotEmpty().WithMessage("ChangePasswordValidator.RequireCurrentPassWord");
            RuleFor(l => l.NewPassWord).NotEmpty().WithMessage("ChangePasswordValidator.RequireNewPassWord");
            RuleFor(l => l.NewPassWord).Matches(new System.Text.RegularExpressions.Regex("(?=^.{8,}$)(?=(.*\\d){2,})(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$"))
                                  .WithMessage("ChangePasswordValidator.InvalidPassWord");
            RuleFor(l => l.ConfirmPassWord)
                .NotEmpty().WithMessage("ChangePasswordValidator.RequireConfirmPassWord")
                .Equal(l => l.NewPassWord).WithMessage("ChangePasswordValidator.PasswordIsMatch");
        }
    }
}
