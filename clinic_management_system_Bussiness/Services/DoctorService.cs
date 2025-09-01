using SharedClasses;
using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Microsoft.Extensions.Options;
using SharedClasses.DTOS.Doctors;
using SharedClasses.DTOS;
using System.Text.Json.Serialization;
using System.Text.Json;
using Azure.Core;
using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
namespace clinic_management_system_Bussiness
{
    public class DoctorService
    {
        private readonly DoctorRepository _repo;
        private readonly UserService _userSerivce;
        private readonly string _connectionString;
        private readonly LoggerService _logger;
        private readonly CurrentUserSevice _currentUserSevice;
        public DoctorService(DoctorRepository repo, IOptions<DatabaseSettings> options,
            UserService userService, LoggerService logger, CurrentUserSevice currentUserSevice)
        {
            _repo = repo;
            _userSerivce = userService;
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
            _currentUserSevice = currentUserSevice;
        }
        public async Task<Result<DoctorDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<DoctorDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetDoctorInfoByIDAsync(id);
        }
        public async Task<Result<DoctorProfileDTO>> GetProfileAsync(int userId)
        {
            return await _repo.GetProfileAsync(userId);
        }
        public async Task<Result<int>> CreateDoctor(CreateDoctorRequestDTO createDoctorRequestDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    Result<int> userResult = await _userSerivce.CreateUserAsync(createDoctorRequestDTO.userDTO, conn, tran);
                    if (!userResult.success)
                    {
                        tran?.Rollback();
                        return CreateFailResponse(userResult.message, userResult.errorCode);
                    }
                    createDoctorRequestDTO.dotorDTO.userId = userResult.data;

                    Result<int> doctorResult = await _repo.AddNewDoctorAsync(createDoctorRequestDTO.dotorDTO, conn, tran);
                    if (!doctorResult.success)
                    {
                        tran?.Rollback();
                        return CreateFailResponse(doctorResult.message, doctorResult.errorCode);
                    }

                    tran.Commit();
                    return doctorResult;


                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }
            }
        }
        public async Task<Result<bool>> UpdateDoctor(UpdateDoctorDTO updateDoctorDTO)
        {
            Result<bool> updateResult = await _repo.UpdateDoctorAsync(updateDoctorDTO);

            if (!updateResult.success)
                return updateResult;
            int? currentUserId = _currentUserSevice.UserId;
            if (currentUserId == null)
                return updateResult;

            Result<UpdateDoctorDTO> oldDataResult = await _repo.GetOldDataAsync(updateDoctorDTO.userId);

            if (!oldDataResult.success)
                return updateResult;

            string oldData = JsonSerializer.Serialize(oldDataResult.data);
            string newData = JsonSerializer.Serialize(updateDoctorDTO);
            CreateAuditLogDTO auditLogDTO = new CreateAuditLogDTO("Doctor", updateDoctorDTO.userId, "Update", (int)currentUserId, oldData, newData);

            await _logger.log(auditLogDTO);

            return updateResult;

        }
        private static Result<int> CreateFailResponse(string message, int errorCode)
        {
            return new Result<int>(false, message, -1, errorCode);
        }
        public async Task<Result<bool>> DeleteDoctorAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteDoctorAsync(id);
        }
        public async Task<Result<List<DoctorInfoDTO>>> GetDoctorsAsync(FilterDoctorDTO filter)
        {
            return await _repo.GetDoctorsAsync(filter);
        }
        public async Task<Result<int>> GetDoctorIdAsync(int userId)
        {
            return await _repo.GetDoctorIdAsync(userId);
        }
    }
}
