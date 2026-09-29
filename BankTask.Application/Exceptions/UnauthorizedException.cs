namespace BankTask.Application.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(
        string errorCode)
        : base(errorCode)
    {
    }
}
