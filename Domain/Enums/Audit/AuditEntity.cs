namespace StockFlow.Domain.Enums.Audit
{
    public enum AuditEntity
    {
        User = 1,
        RefreshToken = 2,

        Product = 3,
        ProductVariant = 4,
        ProductItem = 5,
        Category = 6,

        Supplier = 7,

        StockMovement = 8,

        InboundShipment = 9,
        InboundShipmentItem = 10,

        Customer = 11,

        SalesOrder = 12,
        SalesOrderItem = 13,

        Delivery = 14,
        DeliveryIssue = 15,

        CustomerClaim = 16,
        SupplierClaim = 17,
        QualityIssue = 18
    }
}
