using System.Net;
using System.Text.Json;

namespace aknaIdentityApi.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An error occurred while processing your request.";
            var details = exception.Message;

            // Farklı exception türlerine göre status code ve mesaj özelleştirilebilir
            switch (exception)
            {
                case ArgumentNullException:
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Invalid request parameters.";
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "Unauthorized access.";
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = "The requested resource was not found.";
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                success = false,
                message = message,
                details = details,
                statusCode = (int)statusCode
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
