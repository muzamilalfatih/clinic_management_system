using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
using SharedClasses.DTOS.Patients;
using SharedClasses.Enums;
namespace clinic_management_system_API.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _service;
        private readonly CurrentUserSevice _currentUserService;

        public PatientsController(PatientService service, CurrentUserSevice currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}", Name = "GetPatientByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatientDTO>> GetPatientByID(int id)
        {
            Result<PatientDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreatePatientRequestDTO>> AddPatient(CreatePatientRequestDTO createPatientRequestDTO)
        {
            createPatientRequestDTO.userDTO.CreateUserDTO.createUserRoleDTO.roleId = (int) Roles.Patient;

            Result<int> result = await _service.AddNewPatientAsync(createPatientRequestDTO);
            if (result.success)
            {
                return CreatedAtRoute("GetPatientByID", new { id = result.data }, createPatientRequestDTO);
            }
            return StatusCode(result.errorCode, result.message);
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
     
        public async Task<ActionResult<CreatePatientRequestDTO>> Register(CreatePatientRequestDTO createPatientRequestDTO)
        {
            createPatientRequestDTO.userDTO.CreateUserDTO.createUserRoleDTO.roleId = 9;
            Result<int> result = await _service.AddNewPatientAsync(createPatientRequestDTO);
            if (result.success)
            {
                return CreatedAtRoute("GetPatientByID", new { id = result.data }, createPatientRequestDTO);
            }
            return StatusCode(result.errorCode, result.message);
        }
        [Authorize(Roles= "Admin, SuperAdmin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatientDTO>> UpdatePatient(int id, [FromBody] UpdatePatientDTO UpdateDTO)
        {

            Result<bool> result = await _service.UpdatePatientAsync(UpdateDTO);
            if (result.success)
                return Ok(UpdateDTO);
            return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles = "Patient")]
        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatientDTO>> UpdatePatient( [FromBody] UpdatePatientDTO updateDTO)
        {
            int? currentUserId = _currentUserService.UserId;
            if (currentUserId == null)
                return Unauthorized("Missing user ID in token");

            Result<bool> result = await _service.UpdatePatientAsync((int) currentUserId,  updateDTO);
            if (result.success)
                return Ok(updateDTO);
            return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles ="Admin,SupderAdmin")]
        [HttpDelete("{id}", Name = "DeletePatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> DeletePatient(int id)
        {
            Result<bool> result = await _service.DeletePatientAsync(id);
            if (result.success)
            {
                return Ok($"Patient with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


    }
}
