using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
namespace clinic_management_system_API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
     {
        private readonly RoleService _service;

        public RolesController(RoleService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetRoleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDTO>> GetRoleByID(int id)
        {
            Result<RoleDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDTO>> AddRole(RoleDTO roleDTO)
        {
            Result<int> result = await _service.AddNewRoleAsync(roleDTO);  
            if (result.success)
            {
                return CreatedAtRoute("GetRoleByID", new { id = result.data }, roleDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDTO>> UpdateRole(int id, [FromBody] RoleDTO roleDTO)
        {
            Result<int> result = await _service.UpdateRoleAsync(roleDTO);
            if (result.success)
                return Ok(roleDTO );
           return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeleteRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRole(int id)
        {
            Result<bool> result = await _service.DeleteRoleAsync(id);
            if (result.success)
            {
                return Ok($"Role with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
