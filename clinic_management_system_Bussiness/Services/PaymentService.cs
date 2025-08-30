using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class PaymentService
    {
        private readonly PaymentRepository _repo;

        public PaymentService(PaymentRepository repo)
        {
            _repo = repo;
        }
         public async Task<Result<PaymentDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<PaymentDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetPaymentInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewPaymentAsync(PaymentDTO paymentDTO)
        {
            return await _repo.AddNewPaymentAsync(paymentDTO);
        }

        public async Task<Result<int>> UpdatePaymentAsync(PaymentDTO paymentDTO)
        {
            return await _repo.UpdatePaymentAsync(paymentDTO);
        }

        public  async Task<Result<bool>> DeletePaymentAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeletePaymentAsync(id);
        }

    }
}
