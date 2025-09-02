using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using SharedClasses.DTOS.Doctors;
using SharedClasses.DTOS.LabTechnician;
using SharedClasses.Enums;
namespace clinic_management_system_API.Controllers
{
    [Route("api/labTechnicians")]
     [ApiController]
    [Authorize]
    public class LabTechniciansController : ControllerBase
     {
        private readonly LabTechnicianService _service;
        private readonly CurrentUserSevice _currentUserSevice;

        public LabTechniciansController(LabTechnicianService service, CurrentUserSevice currentUserSevice)
        {
            _service = service;
            _currentUserSevice = currentUserSevice;
        }

        [Authorize(Roles = "Admin,SuperAmdmin")]
        [HttpGet("{id}", Name = "GetLabTechnicianByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabTechnicianDTO>> GetLabTechnicianByID(int id)
        {
            Result<LabTechnicianDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost(Name = "AddLabTechnician")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabTechnicianDTO>> AddLabTechnician([FromBody] CreateLabTechnicianRequestDTO createLabTechnicianResquestDTO)
        {
            createLabTechnicianResquestDTO.UserDTO.CreateUserDTO.createUserRoleDTO.roleId = (int)Roles.LabTechnical;
            Result<int> result = await _service.CreateLabTechnician(createLabTechnicianResquestDTO); 
            if (result.success)
            {
                return CreatedAtRoute("GetLabTechnicianByID", new { id = result.data }, createLabTechnicianResquestDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateDoctorDTO>> UpdateLabTechnician(int id, [FromBody] UpdateLabTechnicianDTO updateDTO)
        {
            Result<bool> result = await _service.UpdateLabTechnicianAsync(updateDTO);
            if (result.success)
                return Ok(updateDTO);

           return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles = "LabTechnical")]
        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateDoctorDTO>> UpdateLabTechnician([FromBody] UpdateLabTechnicianDTO updateDTO)
        {
            int? currentUserId = _currentUserSevice.UserId;
            if (currentUserId == null)
                return Unauthorized("Missing user ID in token");

            Result<bool> result = await _service.UpdateLabTechnicianAsync((int) currentUserId, updateDTO);
            if (result.success)
                return Ok(updateDTO);

            return StatusCode(result.errorCode, result.message);
        }


        [HttpDelete("{id}", Name = "DeleteLabTechnician")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteLabTechnician(int id)
        {
            Result<bool> result = await _service.DeleteLabTechnicianAsync(id);
            if (result.success)
            {
                return Ok($"LabTechnician with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
