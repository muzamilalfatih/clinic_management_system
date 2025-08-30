using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
namespace clinic_management_system_API.Controllers
{
    [Route("api/labDepartments")]
     [ApiController]
    public class LabDepartmentsController : ControllerBase
     {
        private readonly LabDepartmentService _service;

        public LabDepartmentsController(LabDepartmentService serivce)
        {
            _service = serivce;
        }

        [HttpGet("{id}", Name = "GetLabDepartmentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabDepartmentDTO>> GetLabDepartmentByID(int id)
        {
            Result<LabDepartmentDTO> result = await _service.FindAsync(id);
            if (result.success)
            {
                return Ok(result.data);
            }
            return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        }

        [HttpPost(Name = "AddLabDepartment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabDepartmentDTO>> AddLabDepartment(LabDepartmentDTO labDepartmentDTO)
        {
             Result<int> result = await _service._AddNewLabDepartmentAsync(labDepartmentDTO);  
            if (result.success)
            {
                return CreatedAtRoute("GetLabDepartmentByID", new { id = result.data }, labDepartmentDTO);
            }
                return StatusCode(result.errorCode, result.message);
        }

        [HttpPut("{id}", Name = "UpdateLabDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LabDepartmentDTO>> UpdateLabDepartment(int id, [FromBody] LabDepartmentDTO labDepartmentDTO)
        {
            Result<int> result = await _service.UpdateLabDepartmentAsync(labDepartmentDTO);
            if (result.success)
                return Ok(labDepartmentDTO);
           return StatusCode(result.errorCode, result.message);
        }

        [HttpDelete("{id}", Name = "DeleteLabDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteLabDepartment(int id)
        {
            Result<bool> result = await _service.DeleteLabDepartmentAsync(id);
            if (result.success)
            {
                return Ok($"LabDepartment with ID {id} has been deleted.");
            }
            return StatusCode(result.errorCode, result.message);
        }


     }
}
