using SharedClasses;
using SharedClasses.DTOS.Authentication;
using SharedClasses.DTOS.UserRoles;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness
{
    public class AuthunticationService
    {
        private readonly UserService _service;
        private readonly PersonService _personService;
        private readonly PasswordSerivce _passwordSerive;
        public AuthunticationService(UserService service, PersonService personService, PasswordSerivce passwordSerivce )
        {
            _service = service;
            _personService = personService;
            _passwordSerive = passwordSerivce;
        }

        private bool _validateData(LoginDTO loginDTO)
        {
            return loginDTO != null && loginDTO.email != "" && loginDTO.password != "";
        }
        private Result<LoginResponseDTO> createFailResponse(string message, int errorCode)
        {
            return new Result<LoginResponseDTO>(false, message, null, errorCode);
        }
        public async Task<Result<LoginResponseDTO>> Login(LoginDTO loginDTO)
        {
            Result<UserDTO> result = await _service.FindAsync(loginDTO.email);
            if (!result.success)
            {
                if (result.errorCode == 404)
                    return createFailResponse("Invalid email/password", 401);
                return createFailResponse(result.message,result.errorCode);
            }

            if (result.data.roles.All(role  => !role.isActive)) 
                return createFailResponse("User disabled!", 401);

            if (!_passwordSerive.VerifyPassword(result.data.password,loginDTO.password))
                return createFailResponse("Invalid email/password", 401);

            string token = Utility.GenerateJwtToken(result.data);
            Result<PersonDTO> findPersonResult = await _personService.FindAsync(result.data.personId);
            if (!findPersonResult.success)
                return createFailResponse(findPersonResult.message, result.errorCode);

            LoggedUserDTO user = new LoggedUserDTO(result.data.id, _personService.GetDualName(findPersonResult.data),result.data.email, result.data.userName, result.data.roles);
            
            return new Result<LoginResponseDTO>(true, "Login successfully!", new LoginResponseDTO(token, user));
        }
    }
}
