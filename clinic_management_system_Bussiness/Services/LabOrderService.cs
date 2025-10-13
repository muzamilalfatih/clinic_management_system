using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.LabOrder;
using SharedClasses.DTOS.LabOrderTests;
using SharedClasses.DTOS.LabTest;
using SharedClasses.Enums;
using System.Threading.Tasks.Sources;
namespace clinic_management_system_Bussiness
{
    public class LabOrderService
    {

        private readonly LabOrderRepository _repo;
        private readonly AppointmentService _appointmentService;
        private readonly BillService _billService;
        private readonly string _connectionString;
        private readonly LabTestService _labTestService;
        private readonly LabOrderTestService _labOrderTestService;



        public LabOrderService(LabOrderRepository repo, AppointmentService appointmentService,
            BillService billService, IOptions<DatabaseSettings> options, LabTestService labTestService, LabOrderTestService labOrderTestService)
        {
            _connectionString = options.Value.DefaultConnection;
            _repo = repo;
            _appointmentService = appointmentService;
            _billService = billService;
            _labTestService = labTestService;
            _labOrderTestService = labOrderTestService;
        }

        public async Task<Result<LabOrderDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<LabOrderDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetLabOrderInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewAsync(AddNewLabOrderRequestDTO request)
        {

            if (request.NewlabOrder.AppointmentId.HasValue)
            {
                Result<bool> checkAppointmentResult = await _appointmentService.IsValidAsync((int)request.NewlabOrder.AppointmentId);
                if (!checkAppointmentResult.Success)
                    return new Result<int>(false, checkAppointmentResult.Message, -1, checkAppointmentResult.ErrorCode);
                if (!checkAppointmentResult.Data)
                    return new Result<int>(false, "Can process with this appointemnt id!", -1, 400);
            }
            Result<bool> checkPenddingResult = await _repo.HasPenddingAsync(request.NewlabOrder.PersonId, request.NewlabOrder.AppointmentId);
            if (!checkPenddingResult.Success)
                return new Result<int>(false, checkPenddingResult.Message, -1, checkPenddingResult.ErrorCode);

            if (checkPenddingResult.Data)
                return new Result<int>(false, "Already has pendding lab order , NOT Allowed!", -1, 400);

            Result<decimal> totalAmountResult = await _labTestService.GetTotalPriceAsync(request.LabTestIds);
            if (!totalAmountResult.Success)
            {
                return new Result<int>(false, totalAmountResult.Message, -1, totalAmountResult.ErrorCode);
            }
            Result<List<AddNewLabOrderTestDTO>> getOderTestListResult = await _labTestService.GetPricesAsync(request.LabTestIds);
            if (!getOderTestListResult.Success)
            {
                return new Result<int>(false, getOderTestListResult.Message, -1, getOderTestListResult.ErrorCode);
            }


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    Result<int> billResult = await _billService.AddNewBillAsync(totalAmountResult.Data, conn, tran);
                    if (!billResult.Success)
                    {
                        tran?.Rollback();
                        return new Result<int>(false, billResult.Message, -1, billResult.ErrorCode);
                    }
                    Result<int> labOrderResult = await _repo.AddNewLabOrderAsync(request.NewlabOrder, billResult.Data, conn, tran);
                    if (!labOrderResult.Success)
                    {
                        tran?.Rollback();
                        return new Result<int>(false, labOrderResult.Message, -1, labOrderResult.ErrorCode);
                    }
                    Result<bool> addLabOrderTestsResult = await _labOrderTestService.AddNewOrderTestsAsync(getOderTestListResult.Data, labOrderResult.Data, conn, tran);
                    if (!addLabOrderTestsResult.Success)
                    {
                        tran?.Rollback();
                        return new Result<int>(false, addLabOrderTestsResult.Message, -1, addLabOrderTestsResult.ErrorCode);
                    }
                    tran.Commit();
                    return labOrderResult;
                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }
            }
        }

        public async Task<Result<int>> UpdateAsync(LabOrderDTO labOrderDTO)
        {
            return await _repo.UpdateLabOrderAsync(labOrderDTO);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteLabOrderAsync(id);
        }
        public async Task<Result<bool>> ConfirmAsync(int billId, SqlConnection conn, SqlTransaction tran)
        {
            return await _repo.ConfirmAsync(billId, conn, tran);
        }
        public async Task<Result<bool>> ChangeStatus(int id, LabOrderStatus status, SqlConnection conn, SqlTransaction tran)
        {
            return await _repo.ChangeStatusAsync(id, status, conn, tran);
        }
        public async Task<Result<bool>> IsConfirmed(int id)
        {
            return await _repo.IsConfirmed(id);
        }
    }


}
