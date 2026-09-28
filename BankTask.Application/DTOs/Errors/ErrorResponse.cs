namespace BankTask.Application.DTOs.Errors;

public class ErrorResponse
{
    public string ErrorCode { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}