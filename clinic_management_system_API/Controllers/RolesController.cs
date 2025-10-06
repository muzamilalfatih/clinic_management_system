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
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [HttpPost(Name = "AddRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDTO>> AddRole(RoleDTO roleDTO)
        {
            Result<int> result = await _service.AddNewRoleAsync(roleDTO);  
            if (result.Success)
            {
                return CreatedAtRoute("GetRoleByID", new { id = result.Data }, roleDTO);
            }
                return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}", Name = "UpdateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDTO>> UpdateRole(int id, [FromBody] RoleDTO roleDTO)
        {
            Result<int> result = await _service.UpdateRoleAsync(roleDTO);
            if (result.Success)
                return Ok(roleDTO );
           return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}", Name = "DeleteRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRole(int id)
        {
            Result<bool> result = await _service.DeleteRoleAsync(id);
            if (result.Success)
            {
                return Ok($"Role with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


     }
}
