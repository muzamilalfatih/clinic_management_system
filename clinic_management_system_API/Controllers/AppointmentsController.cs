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
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }
        [Authorize(Roles = "Admin,SuperAdmin,Receptionist,Patient")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AppointmentInfoDTO>> AddAppointment(AddNewAppointmentDTO addNewAppointmentDTO)
        {
            Result<int> result = await _service.AddNewAppointmentAsync(addNewAppointmentDTO);
            if (result.success)
            {
                return CreatedAtRoute("GetAppointmentByID", new { id = result.data }, addNewAppointmentDTO);
            }
            return StatusCode(result.errorCode, result.message);
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
            Result<int> result = await _service.UpdateAppointmentAsync(updateAppointmentDTO);

            if (result.success)
                return Ok(updateAppointmentDTO);
            return StatusCode(result.errorCode, result.message);
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
            if (result.success)
            {
                return Ok($"Appointment with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
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
            if (result.success)
            {
                return Ok($"Appointment with ID {id} has been cancel.");
            }
            return StatusCode(result.errorCode, result.message);
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
            if (result.success)
            {
                return Ok($"Appointment with ID {rescheduleDTO.Id} has been rescheduled.");
            }
            return StatusCode(result.errorCode, result.message);
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
            if (result.success)
            {
                return Ok(result.data);
            }
            return StatusCode(result.errorCode, result.message);
        }
    }
}
