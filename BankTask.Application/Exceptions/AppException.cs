namespace BankTask.Application.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(
        string errorCode,
        string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public string ErrorCode { get; }
}