using System.ComponentModel.DataAnnotations;
using BankTask.Domain.Enums;

namespace BankTask.Application.DTOs.Accounts;

public class UpdateAccountRequest
{
    [EnumDataType(typeof(AccountStatus),
        ErrorMessage = "Invalid account status.")]
    public AccountStatus Status { get; set; }
}