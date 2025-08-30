using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class RoleService
    {
        private readonly RoleRepository _repo;

        public RoleService(RoleRepository repo)
        {
            _repo = repo;
        }
       
         public async Task<Result<RoleDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<RoleDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetRoleInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewRoleAsync(RoleDTO roleDTO)
        {
            return await _repo.AddNewRoleAsync(roleDTO);
        }

        public async Task<Result<int>> UpdateRoleAsync(RoleDTO roleDTO)
        {
            return await _repo.UpdateRoleAsync(roleDTO);
        }

        public  async Task<Result<bool>> DeleteRoleAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteRoleAsync(id);
        }
    }
}
