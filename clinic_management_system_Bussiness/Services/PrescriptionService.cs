using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Prescription;
namespace clinic_management_system_Bussiness
{
    public class PrescriptionService
    {
        private readonly PrescriptionRepository _repo;
        private readonly AppointmentService _appointmentService;
        private readonly PrescriptionItemService _prescriptionItemService;
        private readonly string _connectionString;


        public PrescriptionService(PrescriptionRepository repo, AppointmentService appointmentService,
            PrescriptionItemService prescriptionItemService, IOptions<DatabaseSettings> options)
        {
            _repo = repo;
            _appointmentService = appointmentService;
            _prescriptionItemService = prescriptionItemService;
            _connectionString = options.Value.DefaultConnection;
        }

        public async Task<Result<PrescriptionDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<PrescriptionDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewPrescriptionAsync(AddNewPrescriptionRequestDTO request)
        {
            Result<bool> checkResult = await _appointmentService.IsValidAsync(request.PrescriptionDTO.AppointmentId);
            if (!checkResult.Success)
                return new Result<int>(false, checkResult.Message, -1, checkResult.ErrorCode);
            if (!checkResult.Data)
                return new Result<int>(false, "Can process with this appointemnt id!", -1, 400);



            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    Result<int> precriptionResult = await _repo.AddNewAsync(request.PrescriptionDTO, conn, tran);
                    if (!precriptionResult.Success)
                    {
                        tran?.Rollback();
                        return new Result<int>(false, precriptionResult.Message, -1, precriptionResult.ErrorCode);
                    }


                    Result<bool> itemsResult = await _prescriptionItemService.AddNewAsync(request.items, precriptionResult.Data, conn, tran);
                    if (!itemsResult.Success)
                    {
                        tran?.Rollback();
                        return new Result<int>(false, precriptionResult.Message, -1, precriptionResult.ErrorCode);
                    }

                    tran.Commit();
                    return precriptionResult;
                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }
            }


        }

        public async Task<Result<bool>> UpdatePrescriptionAsync(UpdatePrescriptionDTO updateDTO)
        {
            return await _repo.UpdatePrescriptionAsync(updateDTO);
        }

        public async Task<Result<bool>> DeletePrescriptionAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeletePrescriptionAsync(id);
        }


    }
}
