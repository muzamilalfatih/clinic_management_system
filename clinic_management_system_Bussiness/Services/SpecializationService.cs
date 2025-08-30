using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class SpecializationService
    {
        private readonly SpecializationRepository _repo;

        public SpecializationService(SpecializationRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<SpecializationDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<SpecializationDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetSpecializationInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewSpecializationAsync(SpecializationDTO specializationDTO)
        {
            return await _repo.AddNewSpecializationAsync(specializationDTO);
        }

        public async Task<Result<int>> UpdateSpecializationAsync(SpecializationDTO specializationDTO)
        {
            return await _repo.UpdateSpecializationAsync(specializationDTO);
        }

        public  async Task<Result<bool>> DeleteSpecializationAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteSpecializationAsync(id);
        }
    }
}
