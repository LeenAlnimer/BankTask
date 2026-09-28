using System.Globalization;
using System.Text.Json;
using BankTask.Application.Exceptions;
using BankTask.Application.Resources;

namespace BankTask.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (AppException ex)
        {
            _logger.LogWarning(
                ex,
                "Application exception occurred. ErrorCode: {ErrorCode}",
                ex.ErrorCode);

            await HandleAppExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred.");

            await HandleUnexpectedExceptionAsync(context);
        }
    }

    private static async Task HandleAppExceptionAsync(
        HttpContext context,
        AppException exception)
    {
        context.Response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.ContentType = "application/json";

        var culture = GetCulture(context);

        var message =
            Errors.ResourceManager.GetString(
                exception.ErrorCode,
                culture)
            ?? exception.Message;

        var response = new
        {
            errorCode = exception.ErrorCode,
            message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

    private static async Task HandleUnexpectedExceptionAsync(
        HttpContext context)
    {
        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        context.Response.ContentType = "application/json";

        var culture = GetCulture(context);

        var message = culture.TwoLetterISOLanguageName == "ar"
            ? "حدث خطأ غير متوقع."
            : "An unexpected error occurred.";

        var response = new
        {
            errorCode = "INTERNAL_SERVER_ERROR",
            message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

    private static CultureInfo GetCulture(HttpContext context)
    {
        var acceptLanguage =
            context.Request.Headers.AcceptLanguage.ToString();

        if (acceptLanguage.StartsWith(
            "ar",
            StringComparison.OrdinalIgnoreCase))
        {
            return new CultureInfo("ar");
        }

        return new CultureInfo("en");
    }
}