using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Logistics;

namespace StockFlow.Domain.Entities.Sales
{
    public class Customer : BaseEntity
    {
        public string TradeName { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string Cnpj { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<SalesOrder> SalesOrders { get; set; }
            = new List<SalesOrder>();

        public ICollection<Delivery> Deliveries { get; set; }
            = new List<Delivery>();
    }
}
