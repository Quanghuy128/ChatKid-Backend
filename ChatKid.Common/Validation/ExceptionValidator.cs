using FluentValidation;
using FluentValidation.Results;

namespace ChatKid.Common.Validation
{
    public abstract class ExceptionValidator<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var result = base.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            return result;
        }
    }
}
