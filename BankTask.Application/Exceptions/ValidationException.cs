namespace BankTask.Application.Exceptions;

public class ValidationException : AppException
{
    public ValidationException(
        string errorCode,
        string message)
        : base(errorCode, message)
    {
    }
}