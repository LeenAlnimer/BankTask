using System.ComponentModel.DataAnnotations;
using BankTask.Domain.Enums;

namespace BankTask.Application.DTOs.Transactions;

public class CreateTransactionRequest
{
    public Guid? SourceAccountId { get; set; }

    public Guid? DestinationAccountId { get; set; }

    [EnumDataType(typeof(TransactionType))]
    public TransactionType TransactionType { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}