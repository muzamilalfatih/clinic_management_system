using clinic_management_system_Bussiness;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClasses;
using SharedClasses.DTOS.Authentication;

namespace clinic_management_system_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthunticationService _service;

        public AuthController(AuthunticationService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginDTO loginDTO)
        {
            Result<LoginResponseDTO> result = await _service.Login(loginDTO);
            if (!result.Success)
                return StatusCode(result.ErrorCode, result.Message);
            return Ok(result.Data);
        }
    }
}
