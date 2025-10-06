using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
namespace clinic_management_system_API.Controllers
{
    [Route("api/paymentMethods")]
     [ApiController]
    public class PaymentMethodsController : ControllerBase
     {
        private readonly PaymentMethodService _service;

        public PaymentMethodsController(PaymentMethodService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetPaymentMethodByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentMethodDTO>> GetPaymentMethodByID(int id)
        {
            Result<PaymentMethodDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [HttpPost(Name = "AddPaymentMethod")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentMethodDTO>> AddPaymentMethod(PaymentMethodDTO paymentMethodDTO)
        {
            
            Result<int> result = await _service.AddNewPaymentMethodAsync(paymentMethodDTO); 
            if (result.Success)
            {
                return CreatedAtRoute("GetPaymentMethodByID", new { id = result.Data }, paymentMethodDTO);
            }
                return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}", Name = "UpdatePaymentMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentMethodDTO>> UpdatePaymentMethod(int id, [FromBody] PaymentMethodDTO paymentMethodDTO)
        {

            Result<int> result = await _service.UpdatePaymentMethodAsync(paymentMethodDTO);
            if (result.Success)
                return Ok(paymentMethodDTO);
           return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}", Name = "DeletePaymentMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePaymentMethod(int id)
        {
            Result<bool> result = await _service.DeletePaymentMethodAsync(id);
            if (result.Success)
            {
                return Ok($"PaymentMethod with ID {id} has been deleted.");
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


     }
}
