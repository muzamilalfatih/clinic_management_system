using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
namespace clinic_management_system_API.Controllers
{
    [Route("api/auditLogs")]
     [ApiController]
    public class AuditLogsController : ControllerBase
     {
        private readonly AuditLogService _service;

        public AuditLogsController(AuditLogService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetAuditLogByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuditLogDTO>> GetAuditLogByID(int id)
        {
            Result<AuditLogDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

     }
}
