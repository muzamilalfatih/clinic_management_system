using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
namespace clinic_management_system_API.Controllers
{
    [Route("api/appointments")]
     [ApiController]
    public class AppointmentsController : ControllerBase
     {

        private readonly AppointmentService _service;

        public AppointmentsController(AppointmentService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetAppointmentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AppointmentDTO>> GetAppointmentByID(int id)
        {
            Result<AppointmentDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddAppointment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AppointmentDTO>> AddAppointment(AppointmentDTO appointmentDTO)
        {
            Result<int> result = await _service._AddNewAppointmentAsync(appointmentDTO);
            if (result.success)
            {
                return CreatedAtRoute("GetAppointmentByID", new { id = result.data }, appointmentDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdateAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AppointmentDTO>> UpdateAppointment(int id, [FromBody] AppointmentDTO appointmentDTO)
        {
            Result<int> result = await _service._UpdateAppointmentAsync(appointmentDTO);

            if (result.success)
                return Ok(appointmentDTO);
           return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeleteAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            Result<bool> result = await _service.DeleteAppointmentAsync(id);
            if (result.success)
            {
                return Ok($"Appointment with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
