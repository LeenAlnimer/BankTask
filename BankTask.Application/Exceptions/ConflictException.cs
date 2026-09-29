namespace BankTask.Application.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(
        string errorCode)
        : base(errorCode)
    {
    }
}
