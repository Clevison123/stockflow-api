namespace StockFlow.API.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; set; }

        public ValidationException(List<string> errors)
            : base("One or more validation failures occurred.")
        {
            Errors = errors;
        }
    }
}