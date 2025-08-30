using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
namespace clinic_management_system_API.Controllers
{
    [Route("api/prescriptions")]
     [ApiController]
    public class PrescriptionsController : ControllerBase
     {
        private readonly PrescriptionService _service;

        public PrescriptionsController(PrescriptionService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin,Doctor, Patient")]
        [HttpGet("{id}", Name = "GetPrescriptionByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescriptionByID(int id)
        {
            Result<PrescriptionDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddPrescription")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<PrescriptionDTO>> AddPrescription(PrescriptionDTO prescriptionDTO)
        {
            Result<int> result = await _service.AddNewPrescriptionAsync(prescriptionDTO); 
            if (result.success)
            {
                return CreatedAtRoute("GetPrescriptionByID", new { id = result.data }, prescriptionDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}", Name = "UpdatePrescription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrescriptionDTO>> UpdatePrescription(int id, [FromBody] PrescriptionDTO prescriptionDTO)
        {
            Result<int> result = await _service.UpdatePrescriptionAsync(prescriptionDTO);
            if (result.success)
                return Ok(prescriptionDTO);
           return StatusCode(result.errorCode, result.message);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpDelete("{id}", Name = "DeletePrescription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePrescription(int id)
        {
            Result<bool> result = await _service.DeletePrescriptionAsync(id);
            if (result.success)
            {
                return Ok($"Prescription with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
