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
        private Result<LoginResponseDTO> createFailResponse(ResponseMessage message, int errorCode)
        {
            return new Result<LoginResponseDTO>(false, message, null, errorCode);
        }
        public async Task<Result<LoginResponseDTO>> Login(LoginDTO loginDTO)
        {
            Result<UserDTO> result = await _service.FindAsync(loginDTO.email);
            if (!result.Success)
            {
                if (result.ErrorCode == 404)
                    return createFailResponse("Invalid email/password", 401);
                return createFailResponse(result.Message,result.ErrorCode);
            }

            if (result.Data.roles.All(role  => !role.isActive)) 
                return createFailResponse("User disabled!", 401);

            if (!_passwordSerive.VerifyPassword(result.Data.password,loginDTO.password))
                return createFailResponse("Invalid email/password", 401);

            string token = Utility.GenerateJwtToken(result.Data);
            Result<PersonDTO> findPersonResult = await _personService.FindAsync(result.Data.personId);
            if (!findPersonResult.Success)
                return createFailResponse(findPersonResult.Message, result.ErrorCode);

            LoggedUserDTO user = new LoggedUserDTO(result.Data.id, result.Data.personId,result.Data.email, result.Data.userName, result.Data.roles);


            return new Result<LoginResponseDTO>(true, "Login successfully!", new LoginResponseDTO(token, user));
        }
    }
}
