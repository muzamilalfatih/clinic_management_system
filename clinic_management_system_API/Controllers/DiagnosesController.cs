using clinic_management_system_Bussiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using SharedClasses.DTOS.Diagnose;

namespace clinic_management_system_API.Controllers
{
    [Route("api/diagnoses")]
    [ApiController]
    [Authorize]
    public class DiagnosesController : ControllerBase
    {
        private readonly DiagnoseService _service;

        public DiagnosesController(DiagnoseService service)
        {
            _service = service;
        }


        [HttpGet("{id}", Name = "GetDiagnoseByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<DiagnoseInfoDTO>> GetDiagnoseByID(int id)
        {
            Result<DiagnoseInfoDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddDiagnose")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<DiagnoseDTO>> AddDiagnose(AddNewDiagnoseDTO addNew)
        {
            Result<int> result = await _service.AddNewDiagnoseAsync(addNew);
            if (result.success)
            {
                return CreatedAtRoute("GetDiagnoseByID", new { id = result.data }, addNew);
            }
            return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdateDiagnose")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<DiagnoseDTO>> UpdateDiagnose(int id, [FromBody] UpdateDiagnoseDTO update)
        {
            if (id != update.Id)
                return BadRequest("Route id and body id do not match!");

            Result<int> result = await _service.UpdateDiagnoseAsync(update);
            if (result.success)
                return Ok(update);
            return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeleteDiagnose")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteDiagnose(int id)
        {
            Result<bool> result = await _service.DeleteDiagnoseAsync(id);
            if (result.success)
            {
                return Ok($"Diagnose with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }
    }
}
