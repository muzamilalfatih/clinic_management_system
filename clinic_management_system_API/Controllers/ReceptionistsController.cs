using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using SharedClasses.DTOS.Receptionists;
using SharedClasses.Enums;

namespace clinic_management_system_API.Controllers
{
    [Route("api/receptionists")]
    [ApiController]
    public class ReceptionistsController : ControllerBase
    {
        private readonly ReceptionistService _service;
        private readonly CurrentUserSevice _currentUserSevice;

        public ReceptionistsController(ReceptionistService service, CurrentUserSevice currentUserSevice)
        {
            _service = service;
            _currentUserSevice = currentUserSevice;
        }

        [Authorize(Roles = "Admin,SuperAmdmin")]
        [HttpGet("{id}", Name = "GetReceptionistByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReceptionistDTO>> GetReceptionistByID(int id)
        {

            Result<ReceptionistDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        //[Authorize(Roles = "Admin,SuperAmdmin")]
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<ReceptionistDTO>> GetReceptionists([FromQuery] int? specializationId)
        //{
        //    Result<List<ReceptionistInfoDTO>> result = await (specializationId.HasValue ? _service.GetReceptionistsAsync(specializationId.Value)
        //                                        : _service.GetReceptionistsAsync());
        //    if (result.success)
        //    {
        //        return Ok(result.data);
        //    }
        //    return StatusCode(result.errorCode, result.message);
        //}

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost(Name = "AddReceptionist")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FullCreateReceptionistResponseDTO>> AddReceptionist([FromBody] CreateReceptionistRequestDTO createReceptionistRequestDTO)
        {
            createReceptionistRequestDTO.UserDTO.CreateUserDTO.createUserRoleDTO.roleId = (int)Roles.Receptionist;

            Result<int> result = await _service.AddNewReceptionist(createReceptionistRequestDTO);
            if (result.success)
            {
                return CreatedAtRoute("GetReceptionistByID", new { id = result.data }, createReceptionistRequestDTO);
            }
            return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles = "Receptionist")]
        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReceptionistDTO>> UpdateReceptionist([FromBody] UpdateReceptionistDTO updateDTO)
        {
            int? currentUserId = _currentUserSevice.UserId;
            if (currentUserId == null)
                return Unauthorized("Missing user ID in token");

            Result<bool> result = await _service.UpdateReceptionist((int)currentUserId, updateDTO);

            if (!result.success)
                return StatusCode(result.errorCode, result.message);
            return Ok(updateDTO);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReceptionistDTO>> UpdateReceptionist(int id, [FromBody] UpdateReceptionistDTO updateDTO)
        {
            Result<bool> result = await _service.UpdateReceptionist(updateDTO);

            if (!result.success)
                return StatusCode(result.errorCode, result.message);
            return Ok(result.message);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{id}", Name = "DeleteReceptionist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteReceptionist(int id)
        {
            Result<bool> result = await _service.DeleteReceptionistAsync(id);
            if (result.success)
            {
                return Ok($"Receptionist with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }
    }
}
