using System.Net;
using StockFlow.API.Presentation.Responses;
using StockFlow.Application.Exceptions;

namespace StockFlow.API.src.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "An exception occurred: {Message}",
                    exception.Message);

                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = GetStatusCode(exception);

            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse
            {
                Success = false,
                Message = GetMessage(exception),
                Errors = GetErrors(exception)
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ValidationException => HttpStatusCode.BadRequest,

                BadRequestException => HttpStatusCode.BadRequest,

                NotFoundException => HttpStatusCode.NotFound,

                UnauthorizedException => HttpStatusCode.Unauthorized,

                ForbiddenException => HttpStatusCode.Forbidden,

                ConflictException => HttpStatusCode.Conflict,

                BusinessRuleException => HttpStatusCode.BadRequest,

                _ => HttpStatusCode.InternalServerError
            };
        }

        private static string GetMessage(Exception exception)
        {
            return exception switch
            {
                ValidationException => exception.Message,

                BadRequestException => exception.Message,

                NotFoundException => exception.Message,

                UnauthorizedException => exception.Message,

                ForbiddenException => exception.Message,

                ConflictException => exception.Message,

                BusinessRuleException => exception.Message,

                _ => "An unexpected error occurred."
            };
        }

        private static List<string> GetErrors(Exception exception)
        {
            return exception switch
            {
                ValidationException validationException
                    => validationException.Errors,

                BadRequestException
                    => new List<string> { exception.Message },

                NotFoundException
                    => new List<string> { exception.Message },

                UnauthorizedException
                    => new List<string> { exception.Message },

                ForbiddenException
                    => new List<string> { exception.Message },

                ConflictException
                    => new List<string> { exception.Message },

                BusinessRuleException
                    => new List<string> { exception.Message },

                _ => new List<string>
                {
                    "Internal server error."
                }
            };
        }
    }
}