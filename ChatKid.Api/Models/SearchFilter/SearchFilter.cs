namespace ChatKid.Api.Models.SearchFilter
{
    public class SearchFilter
    {
        public string? SearchString { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
