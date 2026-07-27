public class ApplicationValidationException : Exception
{
    public List<string> Errors { get; }

    public ApplicationValidationException(List<string> errors)
        : base("One or more validation failures occurred.")
    {
        Errors = errors;
    }
}