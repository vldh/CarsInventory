using FluentValidation;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(l => l.UserName).NotEmpty().WithMessage("UserModelValidator.RequireUserName");
            RuleFor(l => l.PassWord).NotEmpty().WithMessage("UserModelValidator.RequirePassWord");
            RuleFor(l => l.PassWord).Matches(new System.Text.RegularExpressions.Regex("(?=^.{8,}$)(?=(.*\\d){2,})(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$"))
                                  .WithMessage("ChangePasswordValidator.InvalidPassWord");
            RuleFor(l => l.ConfirmPassWord)
                  .NotEmpty().WithMessage("ChangePasswordValidator.RequireConfirmPassWord")
                  .Equal(l => l.PassWord).WithMessage("ChangePasswordValidator.PasswordIsMatch");

            //RuleFor(l => l.UserName).Matches("^[a-zA-Z0-9 ]+$").WithMessage(localizationService.GetResource("UserModelValidator.InvalidUserName"));
        }
    }
}
