using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Patients;
using SharedClasses.DTOS.People;
using SharedClasses.DTOS.UserRoles;
using SharedClasses.DTOS.Users;
using SharedClasses.Enums;
namespace clinic_management_system_Bussiness
{
    public class PatientService
    {
        private readonly PatientRepository _repo;

        private readonly PersonService _personSerivce;
        private readonly UserService _userSerivce;
        private readonly string _connectionString;
        private readonly UserRoleService _userRoleService;

        public PatientService(PatientRepository repo, PersonService personService, 
            IOptions<DatabaseSettings> options, UserService userSerivce,
            UserRoleService userRoleService)
        {
            _repo = repo;
            _personSerivce = personService;
            _connectionString = options.Value.DefaultConnection;
            _userSerivce = userSerivce;
            _userRoleService = userRoleService;
        }

        public async Task<Result<PatientDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<PatientDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetPatientInfoByIDAsync(id);
        }
        public async Task<Result<PatientProfileDTO>> GetProfileAsync(int userId)
        {
            return await _repo.GetProfileAsync(userId);
        }
        private static Result<int> CreateFailResponse(string message, int errorCode)
        {
            return new Result<int>(false, message, -1, errorCode);
        }

        public async Task<Result<int>> AddNewPatientAsync(CreatePatientRequestDTO createPatientRequestDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    Result<int> userResult = await _userSerivce.CreateUserAsync(createPatientRequestDTO.userDTO, conn, tran);
                    if (!userResult.success)
                    {
                        tran.Rollback();
                        return CreateFailResponse(userResult.message, userResult.errorCode);
                    }
                    createPatientRequestDTO.patientDTO.userId = userResult.data;

                    Result<int> patientResult = await _repo.AddNewPatientAsync(createPatientRequestDTO.patientDTO, conn, tran);
                    if (!patientResult.success)
                    {
                        tran.Rollback();
                        return CreateFailResponse(patientResult.message, patientResult.errorCode);
                    }
                    tran.Commit();
                    return patientResult;

                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }

            }
        }
        public async Task<Result<int>> AddNewPatientAsync(int userId, CreatePatientDTO patientDTO)
        {
            CreateUserRoleDTO createRoleDTO = new CreateUserRoleDTO((int)Roles.Patient, userId);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    Result<int> roleResult = await _userRoleService.AddNewUserRoleAsync(createRoleDTO, conn, tran);
                    if (!roleResult.success)
                    {
                        tran.Rollback();
                        return new Result<int>(false, roleResult.message, -1, roleResult.errorCode);
                    }
                    Result<int> patientResult = await _repo.AddNewPatientAsync(userId, patientDTO, conn, tran);
                    if (!patientResult.success)
                    {
                        tran.Rollback();
                        return patientResult;
                    }
                    tran.Commit();
                    return patientResult;

                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }

            }

        }
        public async Task<Result<bool>> UpdatePatientAsync(UpdatePatientDTO updateDTO)
        {

            return await _repo.UpdatePatientAsync(updateDTO);
        }
        public async Task<Result<bool>> UpdatePatientAsync(int userId, UpdatePatientDTO updateDTO)
        {
            Result<int> getIdResult = await _repo.GetIdAsync(userId);
            if (!getIdResult.success)
                return new Result<bool>(false, getIdResult.message, false, getIdResult.errorCode);
            updateDTO.Id = getIdResult.data;

            return await _repo.UpdatePatientAsync(updateDTO);
        }

        public async Task<Result<bool>> DeletePatientAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeletePatientAsync(id);
        }

    }
}
