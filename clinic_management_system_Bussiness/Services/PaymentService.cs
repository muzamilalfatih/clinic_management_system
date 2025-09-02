using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Appointment;
using SharedClasses.DTOS.Payment;
using SharedClasses.Enums;
namespace clinic_management_system_Bussiness
{
    public class PaymentService
    {
        private readonly PaymentRepository _repo;
        private readonly string _connectionString;
        private readonly BillService _billService;
        private readonly AppointmentService _appointmentService;

        public PaymentService(PaymentRepository repo, IOptions<DatabaseSettings> options, BillService billService, AppointmentService appointmentService)
        {
            _repo = repo;
            _connectionString = options.Value.DefaultConnection;
            _billService = billService;
            _appointmentService = appointmentService;
        }
        public async Task<Result<PaymentInfoDTO>> FindAsync(int id)
        {
            return await _repo.GetPaymentInfoByIDAsync(id);
        }

        public async Task<Result<bool>> PayBill(MakePaymentDTO makePaymentDTO)
        {

            Result<bool> isPaidResult = await _billService.IsPaid(makePaymentDTO.BillId);
            if (!isPaidResult.success)
                return new Result<bool>(false, isPaidResult.message, false, isPaidResult.errorCode);
            if (isPaidResult.data)
                return new Result<bool>(false, "This bill already paid", false, 400);

            Result<decimal> amountResult = await _billService.GetAmount(makePaymentDTO.BillId);
            if (!amountResult.success)
            {
                return new Result<bool>(false, amountResult.message, false, amountResult.errorCode);
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    

                    AddNewPaymentDTO addNew = new AddNewPaymentDTO(makePaymentDTO, amountResult.data);

                    Result<bool> payResult = await _repo.AddNewPaymentAsync(addNew, conn, tran);
                    if (!payResult.success)
                    {
                        tran.Rollback();
                        return new Result<bool>(false, payResult.message, false, payResult.errorCode);
                    }
                    Result<bool> billStatusResult = await _billService.Pay(addNew.BillId, conn, tran);
                    if (!billStatusResult.success)
                    {
                        tran.Rollback();
                        return new Result<bool>(false, billStatusResult.message, false, billStatusResult.errorCode);
                    }

                    Result<bool> appointmentStatusResult = await _appointmentService.ChangeStatus(addNew.BillId, AppointmentStatus.Confirm, conn, tran);
                    if (!appointmentStatusResult.success)
                    {
                        tran.Rollback();
                        return new Result<bool>(false, appointmentStatusResult.message, false, appointmentStatusResult.errorCode);
                    }
                    tran.Commit();
                    return payResult;
                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                }
            }
            //return await _repo.AddNewPaymentAsync(paymentDTO);
        }

      

       
    }
}
