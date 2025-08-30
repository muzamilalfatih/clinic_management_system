using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using SharedClasses.DTOS.LabOrderTests;
namespace clinic_management_system_API.Controllers
{
    [Route("api/labOrderTests")]
     [ApiController]
    public class LabOrderTestsController : ControllerBase
     {

        private readonly LabOrderTestService _service;

        public LabOrderTestsController(LabOrderTestService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetLabOrderTestByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabOrderTestDTO>> GetLabOrderTestByID(int id)
        {
            Result<LabOrderTestDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddLabOrderTest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabOrderTestDTO>> AddLabOrderTest(CreateLabOrderTestDTO createlabOrderTestDTO)
        {
            Result<int> result = await _service.AddNewLabOrderTestAsync(createlabOrderTestDTO);  
            if (result.success)
            {
                return CreatedAtRoute("GetLabOrderTestByID", new { id = result.data }, createlabOrderTestDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdateLabOrderTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabOrderTestDTO>> UpdateLabOrderTest(int id, [FromBody] LabOrderTestDTO labOrderTestDTO)
        {

            Result<int> result = await _service._UpdateLabOrderTestAsync(labOrderTestDTO);
            if (result.success)
                return Ok(labOrderTestDTO);
           return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeleteLabOrderTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteLabOrderTest(int id)
        {
            Result<bool> result = await _service.DeleteLabOrderTestAsync(id);
            if (result.success)
            {
                return Ok($"LabOrderTest with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
