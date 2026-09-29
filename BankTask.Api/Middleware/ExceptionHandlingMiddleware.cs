using System.Globalization;
using System.Text.Json;
using BankTask.Application.DTOs.Errors;
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
        var response = new ErrorResponse
        {
            ErrorCode = exception.ErrorCode,
            Message = message
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

        var message =
            Errors.ResourceManager.GetString("UNEXPECTED_ERROR", culture)
            ?? (culture.TwoLetterISOLanguageName == "ar"
                ? "حدث خطأ غير متوقع."
                : "An unexpected error occurred.");

        var response = new ErrorResponse
        {
            ErrorCode = "UNEXPECTED_ERROR",
            Message = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static CultureInfo GetCulture(HttpContext context)
    {
        var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();

        if (string.IsNullOrWhiteSpace(acceptLanguage))
        {
            return new CultureInfo("en");
        }

        // Take the first language token
        var first = acceptLanguage.Split(',').FirstOrDefault()?.Trim().ToLowerInvariant();

        if (first is null)
            return new CultureInfo("en");

        if (first.StartsWith("ar"))
            return new CultureInfo("ar");

        // default to English for supported/en-US etc.
        return new CultureInfo("en");
    }
}