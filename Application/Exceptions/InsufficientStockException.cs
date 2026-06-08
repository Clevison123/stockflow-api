namespace StockFlow.Application.Exceptions
{
    public class InsufficientStockException
    : BusinessRuleException
    {
        public InsufficientStockException(string message)
            : base(message)
        {
        }
    }
}
