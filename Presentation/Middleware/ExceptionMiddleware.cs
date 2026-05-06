using System.Net;
using System.Text.Json;
using StockFlow.API.Application.Exceptions;
using StockFlow.API.Presentation.Responses;

namespace StockFlow.API.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                BusinessRuleException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            var message = exception switch
            {
                NotFoundException => exception.Message,
                BusinessRuleException => exception.Message,
                _ => "An unexpected error occurred."
            };

            var errors = exception switch
            {
                NotFoundException => new List<string> { exception.Message },
                BusinessRuleException => new List<string> { exception.Message },
                _ => new List<string> { "Internal server error." }
            };

            var response = new ErrorResponse
            {
                Success = false,
                Message = message,
                Errors = errors
            };

            context.Response.StatusCode = (int)statusCode;

            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}