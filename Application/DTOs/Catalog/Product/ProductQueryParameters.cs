namespace StockFlow.Application.DTOs.Catalog.Product
{
    public class ProductQueryParameters
    {
        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public int? SupplierId { get; set; }

        public bool? IsActive { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
