using System.Security.Cryptography;
using System.Text;
using SharedClasses;
using clinic_management_system_DataAccess;
using SharedClasses.DTOS.Users;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using SharedClasses.DTOS.Authentication;
using SharedClasses.DTOS.Doctors;
using SharedClasses.DTOS.Patients;
using SharedClasses.DTOS.People;
using SharedClasses.DTOS.UserRoles;
using Microsoft.Extensions.Options;
using SharedClasses.Enums;
namespace clinic_management_system_Bussiness
{
    public class UserService 
    {
        private readonly UserRepository _repo;
        private readonly UserRoleService _userRoleService;
        private readonly PasswordSerivce _passwordSerivce;
        private readonly PersonService _personService;
        private readonly string _connectionString;

        public UserService(UserRepository repo, UserRoleService userRoleSerivce, PersonService personService, PasswordSerivce passwordSerivce, IOptions<DatabaseSettings> options)
        {
            _repo = repo;
            _userRoleService = userRoleSerivce;
            _passwordSerivce = passwordSerivce;
            _connectionString = options.Value.DefaultConnection;
            _personService = personService;
        }
        public async Task<Result<UserDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<UserDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetUserInfoByIDAsync(id);
        }
        public async Task<Result<UserDTO>> FindAsync(string email)
        {
            if (email == "")
            {
                return new Result<UserDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetUserInfoByEmailAsync(email);
        }
        public async Task<Result<UserProfileDTO>> GetProfileAsync(int userId)
        {
            return await _repo.GetProfileAsync(userId);
        }
        private Result<T> _createFailReponse<T>(string message, int code, T data)
        {
            return new Result<T>(false, message, data, code);
        }
        public async Task<Result<int>> CreateUserAsync(CreateUserRequestDTO createUserRequest, SqlConnection conn, SqlTransaction tran)
        {
            Result<bool> emaiExistenceResult = await _repo.IsUserExistByEmail(createUserRequest.CreateUserDTO.email);
            if (!emaiExistenceResult.success)
                return _createFailReponse<int>(emaiExistenceResult.message, emaiExistenceResult.errorCode, -1);
            if (emaiExistenceResult.data)
                return _createFailReponse<int>("This person already has an account!", 400, -1);
            Result<bool> usernameExistenceReuslt = await _repo.IsUserExistByUserName(createUserRequest.CreateUserDTO.email);
            if (!usernameExistenceReuslt.success)
                return _createFailReponse<int>(usernameExistenceReuslt.message, usernameExistenceReuslt.errorCode, -1);
            if (usernameExistenceReuslt.data)
                return _createFailReponse<int>("This user name is userd by another person!", 409, -1);


            return  await _handleNewUserAsync(createUserRequest, conn, tran);
         
        }
        public async Task<Result<int>> CreateAdmin(CreateUserRequestDTO createUserRequestDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    Result<int> userResult = await CreateUserAsync(createUserRequestDTO, conn, tran);
                    if (!userResult.success)
                    {
                        tran?.Rollback();
                        return userResult;
                    }
                    tran?.Commit();
                    return userResult;
                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }

            }
        }

        private async Task<Result<int>> _handleNewUserAsync(CreateUserRequestDTO createUserRequestDTO, SqlConnection conn, SqlTransaction tran)
        {
            Result<int> personResult = await _personService.AddNewPerson(createUserRequestDTO.CreatePersonDTO, conn, tran);
            if (!personResult.success)
            {
                return _createFailReponse<int>(personResult.message, personResult.errorCode, -1);
            }

            createUserRequestDTO.CreateUserDTO.personId = personResult.data;

            createUserRequestDTO.CreateUserDTO.password= _passwordSerivce.HashPaword(createUserRequestDTO.CreateUserDTO.password);

            Result<int> userResult = await _repo.AddNewUserAsync(createUserRequestDTO.CreateUserDTO, conn, tran);
            if (userResult.success)
            {
                createUserRequestDTO.CreateUserDTO.createUserRoleDTO.userId = userResult.data;
                Result<int> saveResult = await _userRoleService.AddNewUserRoleAsync(createUserRequestDTO.CreateUserDTO.createUserRoleDTO, conn, tran);
                if (!saveResult.success)
                {
                    return _createFailReponse<int>(saveResult.message, saveResult.errorCode, -1);
                }
                
                return userResult;
            }
            return userResult;
        }
        public async Task<Result<bool>> UpdateUserAsync(UpdateUserDTO updateUserDTO)
        {
            updateUserDTO.Password = _passwordSerivce.HashPaword(updateUserDTO.Password);
            return await _repo.UpdateUserAsync(updateUserDTO);
        }
        public async Task<Result<bool>> DeleteUserAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteUserAsync(id);
        }
        public async Task<Result<bool>> AssignNewRoleAsync(CreateUserRoleResquestDTO createUserRoleResquestDTO)
        {
            Result<List<UserRoleDTO>> userRolesResult = await _userRoleService.GetAllUserRolesAsync(createUserRoleResquestDTO.email);
            if (!userRolesResult.success)
                return _createFailReponse<bool>(userRolesResult.message, userRolesResult.errorCode, false);

            List<int> existingRoles = userRolesResult.data.Select(r => r.roleId).ToList();

            if (existingRoles.Contains(createUserRoleResquestDTO.createUserRoleDTO.roleId))
                return _createFailReponse<bool>("This user already has this role.", 400, false);

            bool isNewRolePatient = createUserRoleResquestDTO.createUserRoleDTO.roleId == 9;
            bool userHasPatient = existingRoles.Contains(9);
            int nonPatientRolesCount = existingRoles.Count(r => r != 9);
            bool isNewRoleNonPatient = !isNewRolePatient;

            if (isNewRoleNonPatient && nonPatientRolesCount >= 1)
                return _createFailReponse<bool>("User cannot have more than one non-Patient role.", 400, false);

            createUserRoleResquestDTO.createUserRoleDTO.userId = userRolesResult.data[0].userId;

            return await _userRoleService.AddNewUserRoleAsync(createUserRoleResquestDTO.createUserRoleDTO);

        }
        public async Task<Result<bool>> AssignNewRoleAsync( CreateUserRoleDTO createUserRoleDTO)
        {
            Result<List<UserRoleDTO>> userRolesResult = await _userRoleService.GetAllUserRolesAsync(createUserRoleDTO.userId);
            if (!userRolesResult.success)
                return _createFailReponse<bool>(userRolesResult.message, userRolesResult.errorCode, false);

            List<int> existingRoles = userRolesResult.data.Select(r => r.roleId).ToList();
            int userId = userRolesResult.data[0].userId;
            if (existingRoles.Contains(createUserRoleDTO.roleId))
                return _createFailReponse<bool>("This user already has this role.", 400, false);

            bool isNewRolePatient = createUserRoleDTO.roleId == 9;
            bool userHasPatient = existingRoles.Contains(9);
            int nonPatientRolesCount = existingRoles.Count(r => r != 9);
            bool isNewRoleNonPatient = !isNewRolePatient;

            if (isNewRoleNonPatient && nonPatientRolesCount >= 1)
                return _createFailReponse<bool>("User cannot have more than one non-Patient role.", 400, false);

            return await _userRoleService.AddNewUserRoleAsync(createUserRoleDTO);

        }
        public async Task<Result<bool>> ToggleStatusAsync(ToggleRoleStatusDTO toggleRoleStatusDTO)
        {
            return await _userRoleService.ToggleStatusAsync(toggleRoleStatusDTO);
        }
        public async Task<Result<int>> GetPersonIdAsync(int userId)
        {
            return await _repo.GetPersonIdAsync(userId);
        }
    }
}
