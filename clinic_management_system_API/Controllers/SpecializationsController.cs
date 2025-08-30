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
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddSpecialization")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SpecializationDTO>> AddSpecialization(SpecializationDTO specializationDTO)
        {
            Result<int> result = await _service.AddNewSpecializationAsync(specializationDTO); 
            if (result.success)
            {
                return CreatedAtRoute("GetSpecializationByID", new { id = result.data }, specializationDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdateSpecialization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SpecializationDTO>> UpdateSpecialization(int id, [FromBody] SpecializationDTO specializationDTO)
        {

            Result<int> result = await _service.UpdateSpecializationAsync(specializationDTO);
            if (result.success)
                return Ok(specializationDTO);
           return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeleteSpecialization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteSpecialization(int id)
        {
            Result<bool> result = await _service.DeleteSpecializationAsync(id);
            if (result.success)
            {
                return Ok($"Specialization with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
