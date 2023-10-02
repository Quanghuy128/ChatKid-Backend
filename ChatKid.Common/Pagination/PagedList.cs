namespace ChatKid.Common.Pagination
{
    public class PagedList<T>
    {
        public int PageNumber { get; protected set; }
        public int PageSize { get; protected set; }
        public int TotalItem { get; protected set; } = 0;
        public IEnumerable<T> Items { get; set; } = new List<T>();

        public PagedList(IEnumerable<T> items, int totalItem, PaginationParameters parameters)
        {
            PageNumber = parameters.PageNumber;
            PageSize = parameters.PageSize;
            TotalItem = totalItem;
            Items = items;
        }

        public PagedList(IEnumerable<T> items) : this(items, items.Count(), new PaginationParameters(items.Count(), 1)) { }
    }
}
