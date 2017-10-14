using FluentValidation;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Credential;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Validator
{
    public class RoleModelValidator : AbstractValidator<RoleModel>
    {
        public RoleModelValidator()
        {
            RuleFor(l => l.RoleName).NotEmpty().WithMessage("RoleModelValidator.RequireRoleName");
        }
    }
}
