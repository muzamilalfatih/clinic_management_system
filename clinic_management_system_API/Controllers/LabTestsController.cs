using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using clinic_management_system_DataAccess;
using SharedClasses.DTOS.LabTest;
namespace clinic_management_system_API.Controllers
{
    [Route("api/labTests")]
     [ApiController]
    public class LabTestsController : ControllerBase
     {
        private readonly LabTestService _service;

        public LabTestsController(LabTestService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetLabTestByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabTestDTO>> GetLabTestByID(int id)
        {
            Result<LabTestDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [HttpPost(Name = "AddLabTest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabTestDTO>> AddLabTest(LabTestDTO labTestDTO)
        {
            Result<int> result = await _service.AddNewLabTestAsync(labTestDTO);
            if (result.Success)
            {
                return CreatedAtRoute("GetLabTestByID", new { id = result.Data }, labTestDTO);
            }
                return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}", Name = "UpdateLabTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabTestDTO>> UpdateLabTest(int id, [FromBody] LabTestDTO labTestDTO)
        {
            Result<int> result = await _service.UpdateLabTestAsync(labTestDTO);
            if (result.Success)
                return Ok(labTestDTO);
           return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}", Name = "DeleteLabTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteLabTest(int id)
        {
            Result<bool> result = await _service.DeleteLabTestAsync(id);
            if (result.Success)
            {
                return Ok($"LabTest with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


     }
}
