using SharedClasses;
using SharedClasses.DTOS.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness.Services
{
    
    public  class PersonFacadeService
    {
        private readonly PersonService _personService;
        private readonly UserService _userService;

        public PersonFacadeService(PersonService personService, UserService userService)
        {
            _personService = personService;
            _userService = userService;
        }

        public async Task<Result<bool>> UpdatePersonAsync(int userId, UpdatePersonDTO updateDTO)
        {
            Result<int> getIdResult = await _userService.GetPersonIdAsync(userId);
            if (!getIdResult.Success)
                return new Result<bool>(false, getIdResult.Message, false, getIdResult.ErrorCode);

            updateDTO.id = getIdResult.Data;

            return await _personService.UpdatePersonAsync(updateDTO);  
        }
    }
}
