using BankTask.Domain.Enums;

namespace BankTask.Application.DTOs.Accounts;

public class UpdateAccountRequest
{
    public AccountStatus Status { get; set; }
}