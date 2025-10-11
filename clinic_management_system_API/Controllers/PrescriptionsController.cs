using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
using SharedClasses.DTOS.Prescription;
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
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{id}", Name = "GetPrescriptionByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescriptionByID(int id)
        {
            Result<PrescriptionDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [HttpPost(Name = "AddPrescription")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<PrescriptionDTO>> AddPrescription(AddNewPrescriptionRequestDTO repesut)
        {
            Result<int> result = await _service.AddNewPrescriptionAsync(repesut); 
            if (result.Success)
            {
                return CreatedAtRoute("GetPrescriptionByID", new { id = result.Data }, repesut);
            }
                return StatusCode(result.ErrorCode, result.Message);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}", Name = "UpdatePrescription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrescriptionDTO>> UpdatePrescription(int id, [FromBody] UpdatePrescriptionDTO updateDTO)
        {
            Result<bool> result = await _service.UpdatePrescriptionAsync(updateDTO);
            if (result.Success)
                return Ok(updateDTO);
           return StatusCode(result.ErrorCode, result.Message);
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
            if (result.Success)
            {
                return Ok($"Prescription with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


     }
}
