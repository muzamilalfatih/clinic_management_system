using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
namespace clinic_management_system_API.Controllers
{
    [Route("api/specializations")]
     [ApiController]
    public class SpecializationsController : ControllerBase
     {
        private readonly SpecializationService _service;

        public SpecializationsController(SpecializationService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetSpecializationByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SpecializationDTO>> GetSpecializationByID(int id)
        {
            Result<SpecializationDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [HttpPost(Name = "AddSpecialization")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SpecializationDTO>> AddSpecialization(SpecializationDTO specializationDTO)
        {
            Result<int> result = await _service.AddNewSpecializationAsync(specializationDTO); 
            if (result.Success)
            {
                return CreatedAtRoute("GetSpecializationByID", new { id = result.Data }, specializationDTO);
            }
                return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}", Name = "UpdateSpecialization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SpecializationDTO>> UpdateSpecialization(int id, [FromBody] SpecializationDTO specializationDTO)
        {

            Result<int> result = await _service.UpdateSpecializationAsync(specializationDTO);
            if (result.Success)
                return Ok(specializationDTO);
           return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}", Name = "DeleteSpecialization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteSpecialization(int id)
        {
            Result<bool> result = await _service.DeleteSpecializationAsync(id);
            if (result.Success)
            {
                return Ok($"Specialization with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


     }
}
