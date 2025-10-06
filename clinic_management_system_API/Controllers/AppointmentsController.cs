using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using SharedClasses.DTOS.Appointment;
namespace clinic_management_system_API.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {

        private readonly AppointmentService _service;

        public AppointmentsController(AppointmentService service)
        {
            _service = service;
        }

        //[Authorize(Roles = "Admin,SuperAmdmin,")]
        [HttpGet("{id}", Name = "GetAppointmentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AppointmentInfoDTO>> GetAppointmentByID(int id)
        {
            Result<AppointmentInfoDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }
        [Authorize(Roles = "Admin,SuperAdmin,Receptionist,Patient")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AppointmentInfoDTO>> AddAppointment(AddNewAppointmentRequestDTO addNewAppointmentDTO)
        {
            Result<int> result = await _service.AddNewAppointmentAsync(addNewAppointmentDTO);
            if (result.Success)
            {
                return CreatedAtRoute("GetAppointmentByID", new { id = result.Data }, addNewAppointmentDTO);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [Authorize(Roles = "Admin,SuperAmdmin,Receptionist,Patient")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UpdateAppointmentDTO>> UpdateAppointment(int id, [FromBody] UpdateAppointmentDTO updateAppointmentDTO)
        {
            Result<bool> result = await _service.UpdateAppointmentAsync(updateAppointmentDTO);

            if (result.Success)
                return Ok(updateAppointmentDTO);
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            Result<bool> result = await _service.DeleteAppointmentAsync(id);
            if (result.Success)
            {
                return Ok($"Appointment with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPost("cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CancelAppointment(int id)
        {
            Result<bool> result = await _service.Cancel(id);
            if (result.Success)
            {
                return Ok($"Appointment with ID {id} has been cancel.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPost("reschedule")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Reschedule([FromBody] RescheduleDTO rescheduleDTO)
        {
            Result<bool> result = await _service.Reschedule(rescheduleDTO);
            if (result.Success)
            {
                return Ok($"Appointment with ID {rescheduleDTO.Id} has been rescheduled.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<AppointmentInfoDTO>>> GetAppointments([FromQuery] AppointmentFilterDTO filter)
        {
            Result<List<AppointmentInfoDTO>> result = await _service.GetAllAppointments(filter);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
