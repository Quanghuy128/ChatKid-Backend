using ChatKid.Api.Models;
using ChatKid.Common.Validation;
using FluentValidation;

namespace KMS.Healthcare.TalentInventorySystem.Validators
{
    public class UserLoginValidator : ExceptionValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
