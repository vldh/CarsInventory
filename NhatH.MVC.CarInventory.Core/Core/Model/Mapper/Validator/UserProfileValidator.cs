using FluentValidation;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator
{
    public class UserProfileValidator : AbstractValidator<UserProfileModel>
    {
        public UserProfileValidator()
        {
            RuleFor(l => l.Name).NotEmpty().WithMessage("UserProfileModelValidator.RequireName");
            RuleFor(l => l.Email).EmailAddress().WithMessage("UserProfileModelValidator.InvalidEmail");
            RuleFor(l => l.Telephone).Matches("^[0-9-]*$").WithMessage("UserProfileModelValidator.TelephoneInvalid");
            RuleFor(l => l.Mobile).Matches("^[0-9-]*$").WithMessage("UserProfileModelValidator.MobileInvalid");
        }
    }
}
