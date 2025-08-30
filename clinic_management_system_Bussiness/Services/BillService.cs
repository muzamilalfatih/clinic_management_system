using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class BillService
    {
        private readonly BillRepository _repo;

        public BillService(BillRepository repo)
        {
            _repo = repo;
        }

         public async Task<Result<BillDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<BillDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetBillInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewBillAsync(BillDTO billDTO)
        {
            return await _repo.AddNewBillAsync(billDTO);
        }

        public async Task<Result<int>> UpdateBillAsync(BillDTO billDTO)
        {
            return await _repo.UpdateBillAsync(billDTO);
        }

        public  async Task<Result<bool>> DeleteBillAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteBillAsync(id);
        }
    }
}
