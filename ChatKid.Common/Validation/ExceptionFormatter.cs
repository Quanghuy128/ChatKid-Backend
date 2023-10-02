namespace ChatKid.Common.Validation
{
    public static class ExceptionFormatter
    {
        public static BadRequestErrorResponse Format(this FluentValidation.ValidationException exception)
        {
            var response = new BadRequestErrorResponse();
            foreach (var error in exception.Errors)
            {
                response.Errors.Add(new ErrorDetail()
                {
                    PropertyName = error.PropertyName,
                    ErrorMessage = error.ErrorMessage
                });
            }

            return response;
        }
    }
}
