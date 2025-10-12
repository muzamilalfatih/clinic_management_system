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
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        //[HttpPost(Name = "AddLabOrderTest")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<LabOrderTestDTO>> AddLabOrderTest(AddNewLabOrderTestDTO createlabOrderTestDTO)
        //{
        //    Result<int> result = await _service.AddNewLabOrderTestAsync(createlabOrderTestDTO);  
        //    if (result.Success)
        //    {
        //        return CreatedAtRoute("GetLabOrderTestByID", new { id = result.Data }, createlabOrderTestDTO);
        //    }
        //        return StatusCode(result.ErrorCode, result.Message);
        //}

        //[HttpPut("{id}", Name = "UpdateLabOrderTest")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<LabOrderTestDTO>> UpdateLabOrderTest(int id, [FromBody] LabOrderTestDTO labOrderTestDTO)
        //{

        //    Result<int> result = await _service._UpdateLabOrderTestAsync(labOrderTestDTO);
        //    if (result.Success)
        //        return Ok(labOrderTestDTO);
        //   return StatusCode(result.ErrorCode, result.Message);
        //}

        [HttpDelete("{id}", Name = "DeleteLabOrderTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteLabOrderTest(int id)
        {
            Result<bool> result = await _service.DeleteLabOrderTestAsync(id);
            if (result.Success)
            {
                return Ok($"LabOrderTest with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


     }
}
