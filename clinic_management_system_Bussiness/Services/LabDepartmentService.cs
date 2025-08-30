using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class LabDepartmentService
    {
        private readonly LabDepartmentRepository _repo;

        public LabDepartmentService(LabDepartmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<LabDepartmentDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<LabDepartmentDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetLabDepartmentInfoByIDAsync(id);
        }

        public async Task<Result<int>> _AddNewLabDepartmentAsync(LabDepartmentDTO labDepartmentDTO)
        {
            return await _repo.AddNewLabDepartmentAsync(labDepartmentDTO);
        }

        public async Task<Result<int>> UpdateLabDepartmentAsync(LabDepartmentDTO labDepartmentDTO)
        {
            return await _repo.UpdateLabDepartmentAsync(labDepartmentDTO);
        }

        public  async Task<Result<bool>> DeleteLabDepartmentAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteLabDepartmentAsync(id);
        }
    }
}
