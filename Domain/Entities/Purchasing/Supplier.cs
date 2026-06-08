using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Entities.Common;

namespace StockFlow.Domain.Entities.Purchasing
{
    public class Supplier : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}