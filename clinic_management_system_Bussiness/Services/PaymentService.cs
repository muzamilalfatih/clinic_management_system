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
        private readonly LabOrderService _labOrdeService;

        public PaymentService(PaymentRepository repo, IOptions<DatabaseSettings> options,
            BillService billService, AppointmentService appointmentService, LabOrderService labOrdeService)
        {
            _repo = repo;
            _connectionString = options.Value.DefaultConnection;
            _billService = billService;
            _appointmentService = appointmentService;
            _labOrdeService = labOrdeService;
        }
        public async Task<Result<PaymentInfoDTO>> FindAsync(int id)
        {
            return await _repo.GetPaymentInfoByIDAsync(id);
        }

        public async Task<Result<bool>> PayBill(MakePaymentDTO makePaymentDTO)
        {

            Result<bool> isPaidResult = await _billService.IsPaid(makePaymentDTO.BillId);
            if (!isPaidResult.Success)
                return new Result<bool>(false, isPaidResult.Message, false, isPaidResult.ErrorCode);
            if (isPaidResult.Data)
                return new Result<bool>(false, "This bill already paid", false, 400);

            Result<decimal> amountResult = await _billService.GetAmount(makePaymentDTO.BillId);
            if (!amountResult.Success)
            {
                return new Result<bool>(false, amountResult.Message, false, amountResult.ErrorCode);
            }


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    AddNewPaymentDTO addNew = new AddNewPaymentDTO(makePaymentDTO, amountResult.Data);

                    Result<bool> payResult = await _repo.AddNewPaymentAsync(addNew, conn, tran);
                    if (!payResult.Success)
                    {
                        tran.Rollback();
                        return new Result<bool>(false, payResult.Message, false, payResult.ErrorCode);
                    }
                    Result<bool> billStatusResult = await _billService.Pay(addNew.BillId, conn, tran);
                    if (!billStatusResult.Success)
                    {
                        tran.Rollback();
                        return new Result<bool>(false, billStatusResult.Message, false, billStatusResult.ErrorCode);
                    }

                    Result<bool> checkResult = await _appointmentService.IsExistAsync(makePaymentDTO.BillId, conn, tran);
                    if (!checkResult.Success)
                    {
                        tran.Rollback();
                        return new Result<bool>(false, checkResult.Message, false, checkResult.ErrorCode);
                    }
                    if (checkResult.Data)
                    {
                        Result<bool> appointmentStatusResult = await _appointmentService.ChangeStatus(addNew.BillId, AppointmentStatus.Confirm, conn, tran);
                        if (!appointmentStatusResult.Success)
                        {
                            tran.Rollback();
                            return new Result<bool>(false, appointmentStatusResult.Message, false, appointmentStatusResult.ErrorCode);
                        }
                    }
                    else
                    {
                        Result<bool> labOrderResult = await _labOrdeService.ChangeStatus(makePaymentDTO.BillId, LabOrderStatus.Confirmed, conn, tran);
                        if (!labOrderResult.Success)
                        {
                            tran.Rollback();
                            return new Result<bool>(false, labOrderResult.Message, false, labOrderResult.ErrorCode);
                        }
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
        }




    }
}
