using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Inventory;
using StockFlow.Domain.Entities.Purchasing;

namespace StockFlow.Domain.Entities.Catalog
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string Barcode { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string CountryOfOrigin { get; set; } = string.Empty;

        public int WarrantyMonths { get; set; }

        public bool HasSerialNumber { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public int QuantityInStock { get; set; }

        public int MinimumStock { get; set; } = 5;

        public bool IsActive { get; set; } = true;

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }

        public List<StockMovement> StockMovements { get; set; } = new();
    }
}
