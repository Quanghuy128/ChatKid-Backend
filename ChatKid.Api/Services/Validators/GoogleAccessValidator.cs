using ChatKid.Api.Models.RequestModels;
using ChatKid.Common.Validation;
using FluentValidation;

namespace ChatKid.Api.Services.Validators
{
    public class GoogleAccessValidator : ExceptionValidator<GoogleAccessRequest>
    {
        public GoogleAccessValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty();
        }
    }
}
