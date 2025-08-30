using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS;
using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Receptionists;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness
{
    public class ReceptionistService
    {
        private readonly ReceptionistRepository _repo;
        private readonly PersonService _personSerivce;
        private readonly UserService _userSerivce;
        private readonly string _connectionString;
        private readonly CurrentUserSevice _currentUserSevice;

        public ReceptionistService(ReceptionistRepository repo, PersonService personService, IOptions<DatabaseSettings> options,
            UserService userService, LoggerService logger, CurrentUserSevice currentUserSevice)
        {
            _repo = repo;
            _personSerivce = personService;
            _userSerivce = userService;
            _connectionString = options.Value.DefaultConnection;
            _currentUserSevice = currentUserSevice;
        }
        public async Task<Result<ReceptionistDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<ReceptionistDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetReceptionistInfoByIDAsync(id);
        }
        public async Task<Result<ReceptionistProfileDTO>> GetProfileAsync(int userId)
        {
            return await _repo.GetProfileAsync(userId);
        }
        public async Task<Result<int>> AddNewReceptionist(CreateReceptionistRequestDTO createReceptionistRequestDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();


                    Result<int> userResult = await _userSerivce.CreateUserAsync(createReceptionistRequestDTO.UserDTO, conn, tran);
                    if (!userResult.success)
                    {
                        tran?.Rollback();
                        return CreateFailResponse(userResult.message, userResult.errorCode);
                    }
                    createReceptionistRequestDTO.ReceptionistDTO.UserId = userResult.data;
                    Result<int> ReceptionistResult = await _repo.AddNewReceptionistAsync(createReceptionistRequestDTO.ReceptionistDTO, conn, tran);
                    if (!ReceptionistResult.success)
                    {
                        tran?.Rollback();
                        return CreateFailResponse(ReceptionistResult.message, ReceptionistResult.errorCode);
                    }

                    tran?.Commit();
                    return ReceptionistResult;

                }
                catch
                (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }

            }
        }
        public async Task<Result<bool>> UpdateReceptionist(UpdateReceptionistDTO updateReceptionistDTO)
        {
            return await _repo.UpdateReceptionistAsync(updateReceptionistDTO);

        }
        private static Result<int> CreateFailResponse(string message, int errorCode)
        {
            return new Result<int>(false, message, -1, errorCode);
        }
        public async Task<Result<bool>> DeleteReceptionistAsync(int id)
        {
            return await _repo.DeleteReceptionistAsync(id);
        }
    }
}
