using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ThothDeskCore.Domain;

namespace ThothDeskCore.Infrastructure.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
    {
        private sealed class ExceptionDetails
        {
            public int Status { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string TraceId { get; set; }

            public ExceptionDetails(int status, string title, string type, string traceId)
            {
                Status = status;
                Title = title;
                Type = type;
                TraceId = traceId;
            }
        }

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                await WriteError(context,
                    StatusCodes.Status400BadRequest,
                    "Request was cancelled.",
                    "https://httpstatuses.io/400");
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        //TODO add more details when logging error
        private async Task HandleException(HttpContext context, Exception ex)
        {
            int status;
            string title;
            string type;

            if (ex is NotFoundException)
            {
                status = StatusCodes.Status404NotFound;
                title = ex.Message;
                type = "https://httpstatuses.io/404";
            }

            //todo add more exception types
            else
            {
                status = StatusCodes.Status500InternalServerError;
                title = ex.Message;
                type = "https://httpstatuses.io/500";
            }

            if (status >= 500)
                _logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", context.TraceIdentifier);
            else
                _logger.LogWarning(ex, "Handled exception {Status}. TraceId: {TraceId}", status, context.TraceIdentifier);

            await WriteError(context, status, title, type);
        }

        private static async Task WriteError(HttpContext context, int status, string title, string type)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";

            var payload = new ExceptionDetails(
                status,
                title,
                type,
                context.TraceIdentifier
            );

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(payload, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })
            );
        }
    }
}
