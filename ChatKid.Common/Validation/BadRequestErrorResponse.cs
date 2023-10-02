namespace ChatKid.Common.Validation
{
    public class BadRequestErrorResponse
    {
        public List<ErrorDetail> Errors { get; set; } = new List<ErrorDetail>();
    }

    public class ErrorDetail
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorDetail()
        {
        }

        public ErrorDetail(string message)
        {
            ErrorMessage = message;
        }
    }
}
