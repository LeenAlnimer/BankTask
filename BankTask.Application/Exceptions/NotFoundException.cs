namespace BankTask.Application.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(
        string errorCode)
        : base(errorCode)
    {
    }
}
