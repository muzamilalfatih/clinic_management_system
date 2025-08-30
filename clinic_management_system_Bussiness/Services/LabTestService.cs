using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class LabTestService
    {
        private readonly LabTestRepository _repo;

        public LabTestService(LabTestRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<LabTestDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<LabTestDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetLabTestInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewLabTestAsync(LabTestDTO labTestDTO)
        {
            return await _repo.AddNewLabTestAsync(labTestDTO);
        }

        public async Task<Result<int>> UpdateLabTestAsync(LabTestDTO labTestDTO)
        {
            return await _repo.UpdateLabTestAsync(labTestDTO);
        }

        public async Task<Result<bool>> DeleteLabTestAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteLabTestAsync(id);
        }
        public async Task<Result<float>> GetTotalPrice(List<int> ids)
        {
            return await _repo.GetTotalPrice(ids);
        }
    }
}
