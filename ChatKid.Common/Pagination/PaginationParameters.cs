namespace ChatKid.Common.Pagination
{
    public class PaginationParameters
    {
        public PaginationParameters() { }
        public PaginationParameters(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
