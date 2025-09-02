using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using SharedClasses.DTOS.Appointment;
using SharedClasses.DTOS.Payment;
namespace clinic_management_system_API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _service;

        public PaymentsController(PaymentService service)
        {
            _service = service;
        }

        [HttpGet( Name = "GetPaymentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaymentInfoDTO>> GetPaymentByID([FromQuery] int billId)
        {
            Result<PaymentInfoDTO> result = await _service.FindAsync(billId);
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaymentDTO>> AddPayment(MakePaymentDTO makePaymentDTO)
        {
            Result<bool> result = await _service.PayBill(makePaymentDTO);
            if (result.success)
            {
                return CreatedAtRoute("GetPaymentByID", new { id = result.data }, makePaymentDTO);
            }
            return StatusCode(result.errorCode, result.message);
        }

        
      


    }
}
