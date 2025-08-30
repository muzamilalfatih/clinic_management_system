using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using clinic_management_system_Bussiness;
using SharedClasses.DTOS.People;
using Microsoft.AspNetCore.Authorization;
using clinic_management_system_Bussiness.Services;
namespace clinic_management_system_API.Controllers
{
    [Route("api/people")]
     [ApiController]
    public class PeopleController : ControllerBase
     {
        private readonly PersonService _service;
        private readonly CurrentUserSevice _currentUserSevice;
        private readonly PersonFacadeService _personFacadeService;

        public PeopleController(PersonService service, CurrentUserSevice currentUserSevice, PersonFacadeService personFacadeService)
        {
            _service = service;
            _currentUserSevice = currentUserSevice;
            _personFacadeService = personFacadeService;
        }

        //[HttpGet("{id}", Name = "GetPersonByID")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<PersonDTO>> GetPersonByID(int id)
        //{
        //    Result<PersonDTO> result = await _service.FindAsync(id);
        //    if (result.success)
        //    {
        //        return Ok(result.data);
        //    }
        //    return result.errorCode == 400 ? BadRequest(result.message) : NotFound(result.message);
        //}

        [Authorize]
        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDTO>> UpdatePerson( [FromBody] UpdatePersonRequestDTO updatePersonRequestDTO)
        {
            int? CurrentUserId = _currentUserSevice.userId;
            if (CurrentUserId == null)
                return Unauthorized("Missing user ID in token");


            Result<bool> result = await _personFacadeService.UpdatePersonAsync((int)CurrentUserId, updatePersonRequestDTO);
            if (result.success)
                return Ok(updatePersonRequestDTO);
           return StatusCode(result.errorCode, result.message);
        }

        [Authorize(Roles ="Admin,SuperAdmin,Rceptionist")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDTO>> UpdatePerson(int id, [FromBody] UpdatePersonRequestDTO updatePersonRequestDTO)
        {

            UpdatePersonDTO updatePersonDTO = new UpdatePersonDTO(id, updatePersonRequestDTO);

            Result<bool> result = await _service.UpdatePersonAsync(updatePersonDTO);
            if (result.success)
                return Ok(updatePersonRequestDTO);
            return StatusCode(result.errorCode, result.message);
        }


        //[HttpDelete("{id}", Name = "DeletePerson")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult> DeletePerson(int id)
        //{
        //    Result<bool> result = await _service.DeletePersonAsync(id);
        //    if (result.success)
        //    {
        //        return Ok($"Person with ID {id} has been deleted.");
        //    }
        //    return StatusCode(result.errorCode, result.message);
        //}


     }
}
