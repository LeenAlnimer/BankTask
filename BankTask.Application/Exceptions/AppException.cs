namespace BankTask.Application.Exceptions;

public class AppException : Exception
{
    public AppException(
        string errorCode)
        : base(errorCode)
    {
        ErrorCode = errorCode;
    }

    public string ErrorCode { get; }
}
