namespace StockFlow.API.src.Application.DTOs.Reports
{
    public class StockMovementReportQueryParameters
    {
        public  int? ProductId { get; set; }
        public string? MovementType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
