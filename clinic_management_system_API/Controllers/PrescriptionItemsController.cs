using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
namespace clinic_management_system_API.Controllers
{
    [Route("api/prescriptionItems")]
     [ApiController]
    public class PrescriptionItemsController : ControllerBase
     {
        private readonly PrescriptionItemService _service;

        public PrescriptionItemsController(PrescriptionItemService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetPrescriptionItemByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrescriptionItemDTO>> GetPrescriptionItemByID(int id)
        {
            Result<PrescriptionItemDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddPrescriptionItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrescriptionItemDTO>> AddPrescriptionItem(PrescriptionItemDTO prescriptionItemDTO)
        {
            
            Result<int> result = await _service.AddNewPrescriptionItemAsync(prescriptionItemDTO); 
            if (result.success)
            {
                return CreatedAtRoute("GetPrescriptionItemByID", new { id = result.data }, prescriptionItemDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdatePrescriptionItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrescriptionItemDTO>> UpdatePrescriptionItem(int id, [FromBody] PrescriptionItemDTO prescriptionItemDTO)
        {

            Result<int> result = await _service.UpdatePrescriptionItemAsync(prescriptionItemDTO);
            if (result.success)
                return Ok(prescriptionItemDTO);
           return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeletePrescriptionItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePrescriptionItem(int id)
        {
            Result<bool> result = await _service.DeletePrescriptionItemAsync(id);
            if (result.success)
            {
                return Ok($"PrescriptionItem with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
