using SharedClasses;
using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using SharedClasses.DTOS.PescriptionItem;
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
            return await _repo.GetInfoByIDAsync(id);
        }

        public async Task<Result<bool>> AddNewAsync(List<AddNewPrescriptionItemDTO> itemDTOs, int prescriptionId, SqlConnection conn, SqlTransaction tran)
        {
            return await _repo.AddNewAsync(itemDTOs, prescriptionId, conn, tran);
        }

        public async Task<Result<List<PrescriptionItemDTO>>> GetAllAsync(int prescriptionId)
        {
            return await _repo.GetAllAsync(prescriptionId);
        }
        public async Task<Result<int>> UpdatePrescriptionItemAsync(PrescriptionItemDTO prescriptionItemDTO)
        {
            return await _repo.UpdateAsync(prescriptionItemDTO);
        }
        public  async Task<Result<bool>> DeletePrescriptionItemAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteAsync(id);
        }
    }
}
