using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
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
        public async Task<ActionResult<LabOrderDTO>> AddLabOrder(LabOrderDTO labOrderDTO)
        {
            Result<int> result = await _service._AddNewLabOrderAsync(labOrderDTO);  
            if (result.Success)
            {
                return CreatedAtRoute("GetLabOrderByID", new { id = result.Data }, labOrderDTO);
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
            Result<int> result = await _service._UpdateLabOrderAsync(labOrderDTO);
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
            Result<bool> result = await _service.DeleteLabOrderAsync(id);
            if (result.Success)
            {
                return Ok($"LabOrder with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


     }
}
