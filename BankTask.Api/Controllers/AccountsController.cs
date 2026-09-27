using BankTask.Application.DTOs.Accounts;
using BankTask.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankTask.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AccountResponse>> GetById(Guid id)
    {
        var account = await _accountService.GetByIdAsync(id);

        if (account is null)
        {
            return NotFound();
        }

        return Ok(account);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAll()
    {
        var accounts = await _accountService.GetAllAsync();

        return Ok(accounts);
    }

    [HttpPost]
    public async Task<ActionResult<AccountResponse>> Create(
        CreateAccountRequest request)
    {
        var account = await _accountService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = account.Id },
            account);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateAccountRequest request)
    {
        var updated = await _accountService.UpdateAsync(
            id,
            request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _accountService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}