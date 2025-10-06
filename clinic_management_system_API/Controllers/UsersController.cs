using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
using SharedClasses.DTOS.Authentication;
using SharedClasses.DTOS.Users;
using System.Security.Claims;
using SharedClasses.DTOS.UserRoles;
using SharedClasses.Enums;
namespace clinic_management_system_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;
        private readonly ProfileAggregatorService _profileAggregatorService;
        private readonly CurrentUserSevice _currentUserSevice;
        private readonly UserRoleService _userRoleService;
        public UsersController(UserService service, ProfileAggregatorService aggregatorService, CurrentUserSevice currentUserSevice, UserRoleService userRole)
        {
            _service = service;
            _profileAggregatorService = aggregatorService;
            _currentUserSevice = currentUserSevice;
            _userRoleService = userRole;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserPublicDTO>> GetUserByID(int id)
        {
            Result<UserDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(new UserPublicDTO(result.data));
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserPublicDTO>> GetUser([FromQuery] string email)
        {
            Result<UserDTO> result = await _service.FindAsync(email);
            if (result.success)
            {
                return Ok(new UserPublicDTO(result.data));
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseProfileDTO>> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Unauthorized("Missing user ID in token");

            int CurrentUserId = int.Parse(userIdClaim);

            Result<ResponseProfileDTO> result = await _profileAggregatorService.GetReponseProfileAsync(CurrentUserId);
            if (result.success)
            {
                return Ok(result.data);
            }
            return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles =("Admin,SuperAdmin"))]
        [HttpPatch("{userId}/roles/{roleId}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> ToggleStatus(int userId, int roleId, [FromBody] RoleStatusDTO roleStatusDTO)
        {
            if (roleStatusDTO == null)
                return BadRequest("Request body is missing");

            ToggleRoleStatusDTO toggleRoleStatusDTO = new ToggleRoleStatusDTO(userId, roleId, roleStatusDTO.isActive);

            Result<bool> result = await _service.ToggleStatusAsync(toggleRoleStatusDTO);
            if (result.success)
            {
                return Ok(result.message);
            }
            return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles = ("Admin,SuperAdmin"))]
        [HttpPatch("{userId}/roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RolesForUserDTO>>> GetUserRoles(int userId)
        {
            int? currentUserId = _currentUserSevice.UserId;
            if (currentUserId == null)
                Unauthorized("Id missing in the token!");
            if ((currentUserId) == (int)Roles.Admin && (Roles)userId == Roles.Admin)
                return Forbid("Not allowed to access this info!");

            

            Result<List<RolesForUserDTO>> result = await _userRoleService.GetAllRolesInfoAsync(userId);
            if (result.success)
            {
                return Ok(result.data);
            }
            return StatusCode(result.errorCode, result.message);
        }

        
        [Authorize(Roles="Admin, SuperAdmin")]
        [HttpPost("assign-new-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseProfileDTO>> AssignNewRole(CreateUserRoleResquestDTO createUserRoleResquestDTO)
        {
            Result<bool> result = await _service.AssignNewRoleAsync(createUserRoleResquestDTO);
            if (result.success)
            {
                return Ok(result.message);
            }
            return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles ="SuperAdmin")]
        [HttpPost("add-admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserPublicDTO>> AddAdmin(CreateUserRequestDTO createUserRequestDTO)
        {
            createUserRequestDTO.CreateUserDTO.createUserRoleDTO.roleId = (int) Roles.Admin;

            Result< int> result = await _service.CreateAdmin(createUserRequestDTO);
            if (result.success)
            {
                return CreatedAtRoute("GetUserByID", new { id = result.data }, createUserRequestDTO);
            }
            return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            int? currentUserId = _currentUserSevice.UserId;
                
            if (currentUserId == null) 
                return Unauthorized("Missing user ID in token");
            if ((Roles)id == Roles.Admin && (int) currentUserId == (int) Roles.Admin)
                return Forbid("Admins can't update other admins info.");
                

            Result<bool> result = await _service.UpdateUserAsync(updateUserDTO);
            if (result.success)
                return Ok(updateUserDTO);
            return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> UpdateUser([FromBody] UpdateUserDTO updateDTO)
        {
            int? currentUserId = _currentUserSevice.UserId;

            if (currentUserId == null)
                return Unauthorized("Missing user ID in token");

            updateDTO.Id = (int)currentUserId;

            Result<bool> result = await _service.UpdateUserAsync(updateDTO);
            if (result.success)
                return Ok(updateDTO);
            return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            Result<bool> result = await _service.DeleteUserAsync(id);
            if (result.success)
            {
                return Ok($"User with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


    }
}
