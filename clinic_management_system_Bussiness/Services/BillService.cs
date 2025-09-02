using SharedClasses;
using clinic_management_system_DataAccess;
using SharedClasses.DTOS.Bills;
using Microsoft.Data.SqlClient;
using System.Reflection.Emit;
namespace clinic_management_system_Bussiness
{
    public class BillService
    {
        private readonly BillRepository _repo;

        public BillService(BillRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<BillInfoDTO>> FindAsync(int id)
        {
            return await _repo.GetBillInfoByIDAsync(id);
        }
        public async Task<Result<int>> AddNewBillAsync(decimal amount, SqlConnection conn, SqlTransaction tran)
        {
            return await _repo.AddNewBillAsync(amount, conn, tran);
        }
        public async Task<Result<bool>> DeleteBillAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteBillAsync(id);
        }
        public async Task<Result<bool>> Pay(int id, SqlConnection conn, SqlTransaction tran)
        {
            return await _repo.Pay(id, conn, tran);
        }
    }
}
