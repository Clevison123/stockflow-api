namespace StockFlow.API.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumStock { get; set; } = 5;

        public int CategoryId { get; set; }
        public Category? Category { get; set; } = null;

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; } = null;

        public List<StockMovement> StockMovements { get; set; } = new();
    }
}
