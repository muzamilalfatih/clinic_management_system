using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
namespace clinic_management_system_API.Controllers
{
    [Route("api/userRoles")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class UserRolesController : ControllerBase
    {
        private readonly UserRoleService _service;

        public UserRolesController(UserRoleService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetUserRoleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserRoleDTO>> GetUserRoleByID(int id)
        {
            Result<UserRoleDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddUserRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserRoleDTO>> AddUserRole(UserRoleDTO userRoleDTO)
        {
            Result<int> result = await _service.UpdateUserRoleAsync(userRoleDTO);
            if (result.success)
            {
                return CreatedAtRoute("GetUserRoleByID", new { id = result.data }, userRoleDTO);
            }
            return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdateUserRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserRoleDTO>> UpdateUserRole(int id, [FromBody] UserRoleDTO userRoleDTO)
        {
            Result<int> result = await _service.UpdateUserRoleAsync(userRoleDTO);
            if (result.success)
                return Ok(userRoleDTO);
            return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeleteUserRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserRole(int id)
        {
            Result<bool> result = await _service.DeleteUserRoleAsync(id);
            if (result.success)
            {
                return Ok($"UserRole with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


    }
}
