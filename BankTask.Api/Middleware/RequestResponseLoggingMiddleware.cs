using System.Diagnostics;

namespace BankTask.Api.Middleware;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var request = context.Request;

        var traceId = context.TraceIdentifier;

        _logger.LogInformation(
            "HTTP Request: {Method} {Path} TraceId: {TraceId}",
            request.Method,
            request.Path,
            traceId);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogInformation(
                "HTTP Response: {Method} {Path} responded with {StatusCode} in {ElapsedMilliseconds} ms TraceId: {TraceId}",
                request.Method,
                request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                traceId);
        }
    }
}