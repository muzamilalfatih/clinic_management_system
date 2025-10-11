using clinic_management_system_Bussiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using SharedClasses.DTOS.LabTestParameter;

namespace clinic_management_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabTestParametersController : ControllerBase
    {
        private readonly LabTestParameterService _service;

        public LabTestParametersController(LabTestParameterService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin,SupderAdmin,LabTechnical,Receptionist")]
        [HttpGet("{id}", Name = "GetLabTestParameterByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<LabTestParameterDTO>> GetLabTestParameterByID(int id)
        {
            Result<LabTestParameterDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [Authorize(Roles = "Admin,SupderAdmin,LabTechnical,Receptionist")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<LabTestParameterDTO>>> GetAll([FromQuery] int labTestId)
        {
            Result<List<LabTestParameterDTO>> result = await _service.GetAllAsync(labTestId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }
        [HttpPost(Name = "AddLabTestParameter")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<LabTestParameterDTO>> AddLabTestParameter(AddNewLabTestParameterDTO addNew)
        {
            Result<int> result = await _service.AddNewLabTestParameterAsync(addNew);
            if (result.Success)
            {
                return CreatedAtRoute("GetLabTestParameterByID", new { id = result.Data }, addNew);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}", Name = "UpdateLabTestParameter")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<LabTestParameterDTO>> UpdateLabTestParameter(int id, [FromBody] UpdateLabTestParameterDTO update)
        {
            if (id != update.Id)
                return BadRequest("Route id and body id do not match!");

            Result<int> result = await _service.UpdateLabTestParameterAsync(update);
            if (result.Success)
                return Ok(update);
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}", Name = "DeleteLabTestParameter")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteLabTestParameter(int id)
        {
            Result<bool> result = await _service.DeleteLabTestParameterAsync(id);
            if (result.Success)
            {
                return Ok($"LabTestParameter with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
