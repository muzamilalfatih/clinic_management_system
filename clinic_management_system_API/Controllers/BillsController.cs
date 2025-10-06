using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using SharedClasses.DTOS.Bills;
using Microsoft.AspNetCore.Authorization;
namespace clinic_management_system_API.Controllers
{
    [Route("api/bills")]
    [ApiController]
    [Authorize]
    public class BillsController : ControllerBase
    {

        private readonly BillService _service;
        public BillsController(BillService serivce)
        {
            _service = serivce;
        }

        [Authorize(Roles = "Admin,SuperAmdmin")]
        [HttpGet("{id}", Name = "GetBillByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BillInfoDTO>> GetBillByID(int id)
        {
            Result<BillInfoDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [HttpDelete("{id}", Name = "DeleteBill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteBill(int id)
        {
            Result<bool> result = await _service.DeleteBillAsync(id);
            if (result.Success)
            {
                return Ok($"Bill with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


    }
}
