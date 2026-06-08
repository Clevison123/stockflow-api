namespace StockFlow.Application.DTOs.Sales.Customers
{
    public class CustomerResponseDto
    {
        public int Id { get; set; }

        public string TradeName { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string Cnpj { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}