using clinic_management_system_Bussiness;
using clinic_management_system_Bussiness.Services;
using clinic_management_system_DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SharedClasses;
using SharedClasses.DTOS.DoctorAvailability;
using SharedClasses.DTOS.Doctors;
using SharedClasses.Enums;
using System.Security.Claims;
namespace clinic_management_system_API.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    
    public class DoctorsController : ControllerBase
     {
        private readonly DoctorService _service;
        private readonly CurrentUserSevice _currentUserSevice;
        private readonly DoctorAvailabilityService _availabilityService;

        public DoctorsController(DoctorService service, CurrentUserSevice currentUserService,
            DoctorAvailabilityService availabilityService)
        {
            this._service = service;
            this._currentUserSevice = currentUserService;
            _availabilityService = availabilityService;
        }

        [Authorize(Roles = "Admin,SuperAmdmin")]
        [HttpGet("{id}", Name = "GetDoctorByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorDTO>> GetDoctorByID(int id)
        {
            
            Result<DoctorDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [HttpGet(Name = "GetDoctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorDTO>> GetDoctors([FromQuery] FilterDoctorDTO filter)
        {

            Result<List<DoctorInfoDTO>> result = await _service.GetDoctorsAsync(filter);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost(Name = "AddDoctor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FullCreateDoctorResponseDTO>> AddDoctor([FromBody]CreateDoctorRequestDTO createDoctorRequestDTO)
        {
            createDoctorRequestDTO.userDTO.CreateUserDTO.createUserRoleDTO.roleId = (int)Roles.Doctor;

            Result<int> result = await _service.CreateDoctor(createDoctorRequestDTO);
            if (result.Success)
            {
                return CreatedAtRoute("GetDoctorByID", new { id = result.Data }, createDoctorRequestDTO);
            }
                return StatusCode(result.ErrorCode, result.Message);
        }

        [Authorize(Roles ="Doctor")]
        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorDTO>> UpdateDoctor([FromBody] UpdateDoctorDTO updateDTO)
        {
            int? currentUserId = _currentUserSevice.UserId;
               if (currentUserId == null)
                return Unauthorized("Missing user ID in token");

            Result<bool> result = await _service.UpdateDoctor((int)currentUserId, updateDTO);

            if (!result.Success)
                return StatusCode(result.ErrorCode, result.Message);
            return Ok(updateDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateDoctorDTO>> UpdateDoctor(int id , [FromBody] UpdateDoctorDTO update)
        {   
            Result<bool> result = await _service.UpdateDoctor(update);

            if (!result.Success)
                return StatusCode(result.ErrorCode, result.Message);
            return Ok(update);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{id}", Name = "DeleteDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            Result<bool> result = await _service.DeleteDoctorAsync(id);
            if (result.Success)
            {
                return Ok($"Doctor with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("me/availabilities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AvailabilityInfoDTO>>> GetDoctorAvailabilities()
        {

            int? userId = _currentUserSevice.UserId;
            if (userId == null)
            {
                return Unauthorized("Missing user ID in token");
            }
            Result<int> GetIdResult = await _service.GetDoctorIdAsync((int) userId);
            if (!GetIdResult.Success)
                return StatusCode(GetIdResult.ErrorCode, GetIdResult.Message);

            Result<List<AvailabilityInfoDTO>> allAvailabilitiesResult = await _availabilityService.GetAllDoctorAvailabiltiesAsync(GetIdResult.Data);
            if (allAvailabilitiesResult.Success)
            {
                return Ok(allAvailabilitiesResult.Data);
            }
            return StatusCode(allAvailabilitiesResult.ErrorCode, allAvailabilitiesResult.Message);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost("me/avilabilities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FullCreateDoctorResponseDTO>> AddAvailability([FromBody] List<CreateAvailabilityRequestDTO> createAvailabilitiesDTO)
        {
            int? userId = _currentUserSevice.UserId;
            if (userId == null)
            {
                return Unauthorized("Missing user ID in token");
            }
            Result<int> GetIdResult = await _service.GetDoctorIdAsync((int)userId);
            if (!GetIdResult.Success)
                return StatusCode(GetIdResult.ErrorCode, GetIdResult.Message);
               

            Result<bool> result = await _availabilityService.CreateAvailabiltiesAsync(GetIdResult.Data, createAvailabilitiesDTO);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("me/availabilities{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorDTO>> UpdateDoctorAvailability(int id, [FromBody] UpdateDoctorAvialbilityDTO update)
        {

            Result<bool> result = await _availabilityService.UpdateAvailabilityAsync(update);

            if (!result.Success)
                return StatusCode(result.ErrorCode, result.Message);
            return Ok(result.Message);
        }


        [Authorize(Roles = "Doctor")]
        [HttpDelete("me/availabilities/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDoctorAvailability(int id)
        {
            Result<bool> result = await _availabilityService.DeleteAvailabilityAsync(id);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
