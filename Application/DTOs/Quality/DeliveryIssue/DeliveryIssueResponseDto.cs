using StockFlow.Domain.Enums.Quality;

public class DeliveryIssueResponseDto
{
    public int Id { get; set; }

    public int DeliveryId { get; set; }

    public string DriverName { get; set; } = string.Empty;

    public string VehiclePlate { get; set; } = string.Empty;

    public int SalesOrderId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public DeliveryIssueType IssueType { get; set; }

    public string Description { get; set; } = string.Empty;

    public bool IsResolved { get; set; }

    public DateTime OccurredAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public string ResolutionNotes { get; set; } = string.Empty;
}