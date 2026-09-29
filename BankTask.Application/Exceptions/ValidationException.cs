namespace BankTask.Application.Exceptions;

public class ValidationException : AppException
{
    public ValidationException(
        string errorCode)
        : base(errorCode)
    {
    }
}
