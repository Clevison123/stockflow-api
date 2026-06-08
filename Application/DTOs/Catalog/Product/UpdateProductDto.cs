namespace StockFlow.Application.DTOs.Catalog.Product
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int MinimumStock { get; set; }

        public int WarrantyMonths { get; set; }
    }
}
