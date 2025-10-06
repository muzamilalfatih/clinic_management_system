using clinic_management_system_Bussiness;
using clinic_management_system_Bussiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using SharedClasses.DTOS.LabDevices;
using SharedClasses.Enums;

namespace clinic_management_system_API.Controllers
{
    [Route("api/labDevicess")]
    [ApiController]
    //[Authorize]
    public class LabDevicesController : ControllerBase
    {
        private readonly LabDeviceService _service;

        public LabDevicesController(LabDeviceService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}", Name = "GetLabDeviceByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabDeviceDTO>> GetLabDevicetByID(int id)
        {
            Result<LabDeviceDTO> result = await _service.FindAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabDeviceDTO>> Search([FromQuery] int? id, [FromQuery] string? name)
        {
            if (id == null && string.IsNullOrWhiteSpace(name))
                return BadRequest("You must provide either 'id' or 'name'.");

            Result<LabDeviceDTO> result = null;

            if (id.HasValue)
                result = await _service.FindAsync((int)id);
            else
                result = await _service.FindAsync(name);
           
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
        public async Task<ActionResult<CreateLabDeviceDTO>> AddLabDevice(CreateLabDeviceDTO createDTO)
        {

            Result<int> result = await _service.AddNewAsync(createDTO);
            if (result.Success)
            {
                return CreatedAtRoute("GetLabDeviceByID", new { id = result.Data }, createDTO);
            }
            return StatusCode(result.ErrorCode, new {Message =  result.Message});
        }




        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<LabDeviceDTO>> UpdateLabDevice(int id, [FromBody] UpdateLabDeviceDTO UpdateDTO)
        {

            Result<bool> result = await _service.UpdateAsync(UpdateDTO);
            if (result.Success)
                return Ok(UpdateDTO);
            return StatusCode(result.ErrorCode, result.Message);
        }

       
        

    }
}
