using SharedClasses;
using clinic_management_system_DataAccess;
using System.Net.NetworkInformation;
using SharedClasses.DTOS.LabOrderTests;
namespace clinic_management_system_Bussiness
{
    public class LabOrderTestService
    {
        private readonly LabOrderTestRepository _repo;

        public LabOrderTestService(LabOrderTestRepository repo)
        {
            _repo = repo;
        }

         public async Task<Result<LabOrderTestDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<LabOrderTestDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetLabOrderTestInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewLabOrderTestAsync(CreateLabOrderTestDTO createLabOrderTestDTO)
        {
            return await _repo.AddNewLabOrderTestAsync(createLabOrderTestDTO);
        }

        public async Task<Result<int>> _UpdateLabOrderTestAsync(LabOrderTestDTO labOrderTestDTO)
        {
            return await _repo.UpdateLabOrderTestAsync(labOrderTestDTO);
        }

        public  async Task<Result<bool>> DeleteLabOrderTestAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteLabOrderTestAsync(id);
        }
    }
}
