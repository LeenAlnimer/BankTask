using BankTask.Application.DTOs.Transactions;
using BankTask.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankTask.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(
        ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionResponse>> GetById(
        Guid id)
    {
        var transaction =
            await _transactionService.GetByIdAsync(id);

        if (transaction is null)
        {
            return NotFound();
        }

        return Ok(transaction);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetAll()
    {
        var transactions =
            await _transactionService.GetAllAsync();

        return Ok(transactions);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResponse>> Create(
        CreateTransactionRequest request)
    {
        var transaction =
            await _transactionService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = transaction.Id },
            transaction);
    }
}