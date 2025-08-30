using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.LabTechnician;
using System.Net.NetworkInformation;
namespace clinic_management_system_Bussiness
{
    public class LabTechnicianService
    {
        private readonly LabTechnicianRepository _repo;
        private readonly UserService _userSerivce;
        private readonly string _connectionString;

        public LabTechnicianService(LabTechnicianRepository repo, IOptions<DatabaseSettings> options,
            UserService userService)
        {
            _repo = repo;
            _userSerivce = userService;
            _connectionString = options.Value.DefaultConnection;
        }

        public async Task<Result<LabTechnicianDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<LabTechnicianDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetLabTechnicianInfoByIDAsync(id);
        }

        public async Task<Result<LabTechnicianProfileDTO>> GetProfile(int userId)
        {
            return await _repo.GetProfileAsync(userId);
        }
        public async Task<Result<int>> CreateLabTechnician(CreateLabTechnicianRequestDTO createLabTechnicianRequestDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    Result<int> userResult = await _userSerivce.CreateUserAsync(createLabTechnicianRequestDTO.UserDTO, conn, tran);
                    if (!userResult.success)
                    {
                        tran?.Rollback();
                        return CreateFailResponse(userResult.message, userResult.errorCode);
                    }
                    createLabTechnicianRequestDTO.LabTechnicianDTO.UserId = userResult.data;

                    Result<int> labTechnicianResult = await _repo.AddNewLabTechnicianAsync(createLabTechnicianRequestDTO.LabTechnicianDTO, conn, tran);
                    if (!labTechnicianResult.success)
                    {
                        tran?.Rollback();
                        return CreateFailResponse(labTechnicianResult.message, labTechnicianResult.errorCode);
                    }

                    tran.Commit();
                    return labTechnicianResult;

                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return CreateFailResponse("An unexpected error occurred on the server.", 500);
                }
            }

        }
        public async Task<Result<bool>> UpdateLabTechnicianAsync(UpdateLabTechnicianDTO updateLabTechnicianDTO)
        {
            return await _repo.UpdateLabTechnicianAsync(updateLabTechnicianDTO);
        }
        public async Task<Result<bool>> DeleteLabTechnicianAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteLabTechnicianAsync(id);
        }
        private Result<int> CreateFailResponse(string message, int errorCode)
        {
            return new Result<int>(false, message, -1, errorCode);
        }
    }
}
