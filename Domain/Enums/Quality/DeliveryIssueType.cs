namespace StockFlow.Domain.Enums.Quality
{
    public enum DeliveryIssueType
    {
        MissingItems = 1,

        DamagedItems = 2,

        WrongItems = 3,

        LateDelivery = 4,

        CustomerRefused = 5,

        VehicleBreakdown = 6,

        Other = 7
    }
}
