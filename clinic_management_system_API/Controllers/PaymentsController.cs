using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
namespace clinic_management_system_API.Controllers
{
    [Route("api/payments")]
     [ApiController]
    public class PaymentsController : ControllerBase
     {
        private readonly PaymentService _service;

        public PaymentsController(PaymentService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetPaymentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentDTO>> GetPaymentByID(int id)
        {
            Result<PaymentDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddPayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentDTO>> AddPayment(PaymentDTO paymentDTO)
        {
            Result<int> result = await _service.AddNewPaymentAsync(paymentDTO);  
            if (result.success)
            {
                return CreatedAtRoute("GetPaymentByID", new { id = result.data }, paymentDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdatePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentDTO>> UpdatePayment(int id, [FromBody] PaymentDTO paymentDTO)
        {
            Result<int> result = await _service.UpdatePaymentAsync(paymentDTO);
            if (result.success)
                return Ok(paymentDTO);
           return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeletePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePayment(int id)
        {
            Result<bool> result = await _service.DeletePaymentAsync(id);
            if (result.success)
            {
                return Ok($"Payment with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
