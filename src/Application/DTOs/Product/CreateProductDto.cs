namespace StockFlow.API.src.Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumStock { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
    }
}
