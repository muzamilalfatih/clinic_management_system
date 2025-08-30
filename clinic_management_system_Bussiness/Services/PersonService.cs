using SharedClasses;
using clinic_management_system_DataAccess;
using SharedClasses.DTOS.People;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
namespace clinic_management_system_Bussiness
{
    public class PersonService
    {
        private readonly PersonRepository _repo;
        
        public PersonService(PersonRepository repo)
        {
            _repo = repo;
        }

        
        public async Task<Result<PersonDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<PersonDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetPersonInfoByIDAsync(id);
        }
        public async Task<Result<PersonProfileDTO>> GetProfileAsync(int userId)
        {
            return await _repo.GetProfileAsync(userId);
        }
        public string GetFullName(PersonDTO personDTO)
        {
            return personDTO.firstName + " " + personDTO.secondName + " " + personDTO.thirdName + " " + personDTO.lastName;
        }

        public string GetDualName(PersonDTO personDTO)
        {
            return personDTO.firstName + " " + personDTO.secondName;
        }
        public  async Task<Result<int>> AddNewPerson(CreatePersonDTO createPersonDTO, SqlConnection conn, SqlTransaction tran)
        {
           // add validation here  
            return await _repo.AddNewPersonAsync(createPersonDTO, conn, tran);
        }
        public async Task<Result<bool>> UpdatePersonAsync(UpdatePersonDTO updatePersonDTO)
        {
            return await _repo.UpdatePersonAsync(updatePersonDTO);
        }

        public  async Task<Result<bool>> DeletePersonAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeletePersonAsync(id);
        }

    }
}
