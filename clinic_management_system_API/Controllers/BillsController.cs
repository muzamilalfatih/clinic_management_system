using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
namespace clinic_management_system_API.Controllers
{
    [Route("api/bills")]
     [ApiController]
    public class BillsController : ControllerBase
     {

        private readonly BillService _service;
        public BillsController(BillService serivce)
        {
            _service = serivce;
        }

        [HttpGet("{id}", Name = "GetBillByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BillDTO>> GetBillByID(int id)
        {
            Result<BillDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddBill")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BillDTO>> AddBill(BillDTO billDTO)
        {
            Result<int> result = await _service.AddNewBillAsync(billDTO);  
            if (result.success)
            {
                return CreatedAtRoute("GetBillByID", new { id = result.data } , billDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdateBill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BillDTO>> UpdateBill(int id, [FromBody] BillDTO billDTO)
        {
            Result<int> result = await _service.UpdateBillAsync(billDTO);
            if (result.success)
                return Ok(billDTO);
           return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeleteBill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteBill(int id)
        {
            Result<bool> result = await _service.DeleteBillAsync(id);
            if (result.success)
            {
                return Ok($"Bill with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
