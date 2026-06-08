namespace StockFlow.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors)
            : base("One or more validation failures occurred.")
        {
            Errors = errors;
        }
    }
}