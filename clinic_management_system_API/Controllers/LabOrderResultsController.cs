using clinic_management_system_Bussiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using SharedClasses.DTOS.LabOrderResults;

namespace clinic_management_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabOrderResultsController : ControllerBase
    {
        private readonly LabOrderResultService _service;

        public LabOrderResultsController(LabOrderResultService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}", Name = "GetLabOrderResultByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabOrderResultDTO>> GetLabOrderResulttByID(int id)
        {
            Result<LabOrderResultDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LabOrderResultDTO>>> Search([FromQuery] int labOrderTestId)
        {
            Result<List<LabOrderResultDTO>> result = await _service.GetAllAsync(labOrderTestId);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AddNewOrderResultDTO>> AddLabOrderResult(List<AddNewOrderResultDTO> results)
        {

            Result<bool> result = await _service.AddNewAsync(results);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }




        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<LabOrderResultDTO>> UpdateLabOrderResult(int id, [FromBody] UpdateLabOrderResultDTO UpdateDTO)
        {

            Result<bool> result = await _service.UpdateAsync(UpdateDTO);
            if (result.Success)
                return Ok(UpdateDTO);
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
