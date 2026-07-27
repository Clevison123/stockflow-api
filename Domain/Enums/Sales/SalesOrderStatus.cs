namespace StockFlow.Domain.Enums.Sales
{
    public enum SalesOrderStatus
    {
        Pending = 1,

        Approved = 2,

        AwaitingStock = 3,

        Picking = 4,

        ReadyForShipment = 5,

        Shipped = 6,

        Delivered = 7,

        Cancelled = 8
    }
}