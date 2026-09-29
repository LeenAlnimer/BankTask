namespace BankTask.Application.DTOs.Errors;

public class ErrorResponse
{
    public string ErrorCode { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    // Trace identifier to correlate logs and responses
    public string TraceId { get; set; } = string.Empty;
}