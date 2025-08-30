using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class PaymentMethodService
    {
        private readonly PaymentMethodRepository _repo;

        public PaymentMethodService(PaymentMethodRepository repo)
        {

        }
        public async Task<Result<PaymentMethodDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<PaymentMethodDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetPaymentMethodInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewPaymentMethodAsync(PaymentMethodDTO paymentMethodDTO)
        {
            return await _repo.AddNewPaymentMethodAsync(paymentMethodDTO);
        }

        public async Task<Result<int>> UpdatePaymentMethodAsync(PaymentMethodDTO paymentMethodDTO)
        {
            return await _repo.UpdatePaymentMethodAsync(paymentMethodDTO);
        }

        public  async Task<Result<bool>> DeletePaymentMethodAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeletePaymentMethodAsync(id);
        }

    }
}
