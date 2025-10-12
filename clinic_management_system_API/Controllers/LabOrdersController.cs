using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using SharedClasses.DTOS.LabOrder;
namespace clinic_management_system_API.Controllers
{
    [Route("api/labOrders")]
     [ApiController]
    public class LabOrdersController : ControllerBase
     {
        private readonly LabOrderService _service;

        public LabOrdersController(LabOrderService service)
        {
            _service = service;
        }


        [HttpGet("{id}", Name = "GetLabOrderByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabOrderDTO>> GetLabOrderByID(int id)
        {
            Result<LabOrderDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [HttpPost(Name = "AddLabOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabOrderDTO>> AddLabOrder(AddNewLabOrderRequestDTO request)
        {
            if (request.NewlabOrder.PersonId == null && request.NewlabOrder.AppointmentId == null)
                return BadRequest(new ResponseMessage("Please provide appointment id or person id."));
            
            Result<int> result = await _service.AddNewAsync(request);  
            if (result.Success)
            {
                return CreatedAtRoute("GetLabOrderByID", new { id = result.Data }, request);
            }
                return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}", Name = "UpdateLabOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabOrderDTO>> UpdateLabOrder(int id, [FromBody] LabOrderDTO labOrderDTO)
        {
            Result<int> result = await _service.UpdateAsync(labOrderDTO);
            if (result.Success)
                return Ok(labOrderDTO);
           return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}", Name = "DeleteLabOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteLabOrder(int id)
        {
            Result<bool> result = await _service.DeleteAsync(id);
            if (result.Success)
            {
                return Ok($"LabOrder with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


     }
}
