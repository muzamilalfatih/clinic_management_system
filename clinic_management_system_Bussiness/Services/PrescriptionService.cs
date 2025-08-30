using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class PrescriptionService
    {
        private readonly PrescriptionRepository _repo;

        public PrescriptionService(PrescriptionRepository repo)
        {
            _repo = repo;
        }

         public async Task<Result<PrescriptionDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<PrescriptionDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetPrescriptionInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewPrescriptionAsync(PrescriptionDTO prescriptionDTO)
        {
            return await _repo.AddNewPrescriptionAsync(prescriptionDTO);
        }

        public async Task<Result<int>> UpdatePrescriptionAsync(PrescriptionDTO prescriptionDTO)
        {
            return await _repo.UpdatePrescriptionAsync(prescriptionDTO);
        }

        public  async Task<Result<bool>> DeletePrescriptionAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeletePrescriptionAsync(id);
        }


    }
}
