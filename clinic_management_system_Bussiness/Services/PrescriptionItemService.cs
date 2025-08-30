using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class PrescriptionItemService
    {
        private readonly PrescriptionItemRepository _repo;

        public PrescriptionItemService(PrescriptionItemRepository repo)
        {
            _repo = repo;
        }

         public async Task<Result<PrescriptionItemDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<PrescriptionItemDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetPrescriptionItemInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewPrescriptionItemAsync(PrescriptionItemDTO prescriptionItemDTO)
        {
            return await _repo.AddNewPrescriptionItemAsync(prescriptionItemDTO);
        }

        public async Task<Result<int>> UpdatePrescriptionItemAsync(PrescriptionItemDTO prescriptionItemDTO)
        {
            return await _repo.UpdatePrescriptionItemAsync(prescriptionItemDTO);
        }

        public  async Task<Result<bool>> DeletePrescriptionItemAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeletePrescriptionItemAsync(id);
        }
    }
}
