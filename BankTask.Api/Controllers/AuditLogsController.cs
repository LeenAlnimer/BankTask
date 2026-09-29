using BankTask.Application.DTOs.AuditLogs;
using BankTask.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankTask.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogsController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuditLogResponse>> GetById(Guid id)
    {
        var auditLog = await _auditLogService.GetByIdAsync(id);
        return Ok(auditLog);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditLogResponse>>> GetAll()
    {
        var auditLogs =
            await _auditLogService.GetAllAsync();

        return Ok(auditLogs);
    }
}