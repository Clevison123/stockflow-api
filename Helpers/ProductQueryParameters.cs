namespace StockFlow.API.Helpers
{
    public class ProductQueryParameters
    {
        public string? Search { get; set; }

        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }

        public string? SortBy { get; set; } = "name";
        public string? SortDirection { get; set; } = "asc";

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
