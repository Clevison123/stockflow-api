namespace StockFlow.Application.Exceptions
{
    public class TokenExpiredException
    : UnauthorizedException
    {
        public TokenExpiredException(string message)
            : base(message)
        {
        }
    }
}
