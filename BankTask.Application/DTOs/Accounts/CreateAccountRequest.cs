using System.ComponentModel.DataAnnotations;
using BankTask.Domain.Enums;

namespace BankTask.Application.DTOs.Accounts;

public class CreateAccountRequest
{
    public Guid UserId { get; set; }

    [EnumDataType(typeof(Currency),
        ErrorMessage = "Invalid currency.")]
    public Currency Currency { get; set; }

    [EnumDataType(typeof(AccountType),
        ErrorMessage = "Invalid account type.")]
    public AccountType AccountType { get; set; }
}