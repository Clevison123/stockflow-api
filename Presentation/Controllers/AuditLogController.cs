using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.API.DTOs.Audit;
using StockFlow.API.Infrastructure.Data;

namespace StockFlow.API.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Cashier")] // Após testes remova o cashier
    [ApiExplorerSettings(GroupName = "AuditLog")]
    public class AuditLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuditLogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _context.AuditLogs
                .OrderByDescending(a => a.Timestamp)
                .Take(100)
                .ToListAsync();

            var logsDto = logs.Select(a => new AuditLogDto
            {
                UserEmail = a.UserEmail,
                Action = a.Action,
                EntityName = a.EntityName,
                EntityId = a.EntityId,
                Timestamp = a.Timestamp
            });

            return Ok(logsDto);
        }
    }
}