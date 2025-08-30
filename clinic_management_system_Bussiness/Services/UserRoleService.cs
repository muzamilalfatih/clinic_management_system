using SharedClasses;
using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using SharedClasses.DTOS.UserRoles;
namespace clinic_management_system_Bussiness
{
    public class UserRoleService
    {
        private readonly UserRoleRepository _repo;

        public UserRoleService(UserRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<UserRoleDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<UserRoleDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetUserRoleInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewUserRoleAsync(CreateUserRoleDTO createUserRoleDTO, SqlConnection conn, SqlTransaction tran)
        {
            return await _repo.AddNewUserRoleAsync(createUserRoleDTO, conn, tran);
        }
        public async Task<Result<bool>> AddNewUserRoleAsync(CreateUserRoleDTO createUserRoleDTO)
        {
            return await _repo.AddNewUserRoleAsync(createUserRoleDTO);
        }
        public async Task<Result<int>> UpdateUserRoleAsync(UserRoleDTO userRoleDTO)
        {
            return await _repo.UpdateUserRoleAsync(userRoleDTO);
        }
        public  async Task<Result<bool>> DeleteUserRoleAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteUserRoleAsync(id);
        }
        public async Task<Result<List<UserRoleDTO>>> GetAllUserRolesAsync(string email)
        {
            return await _repo.GetAllUserRolesAsync(email);
        }
        public async Task<Result<List<UserRoleDTO>>> GetAllUserRolesAsync(int userId)
        {
            return await _repo.GetAllUserRolesAsync(userId);
        }
        public async Task<Result<bool>> ToggleStatusAsync(ToggleRoleStatusDTO toggleRoleStatusDTO)
        {
            return await _repo.ToggleStatusAsync(toggleRoleStatusDTO);
        }
        public async Task<Result<List<RolesForUserDTO>>> GetAllRolesInfoAsync(int  userId)
        {
            return await _repo.GetAllRolesInfoAsync(userId);
        }
    }
}
