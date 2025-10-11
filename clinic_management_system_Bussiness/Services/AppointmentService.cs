using Azure.Core;
using clinic_management_system_Bussiness.Services;
using clinic_management_system_DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Appointment;
using SharedClasses.DTOS.DoctorAvailability;
using SharedClasses.Enums;
using System.Text.RegularExpressions;
namespace clinic_management_system_Bussiness
{
    public class AppointmentService
    {
        
        private readonly AppointmentRepository _repo;
        private readonly DoctorAvailabilityService _availabilityService;
        private readonly string _connectionString;
        private readonly DoctorService _doctorService;
        private readonly BillService _billService;

        public AppointmentService(AppointmentRepository repo, DoctorAvailabilityService doctorAvailabilityService
            , IOptions<DatabaseSettings> options, DoctorService doctorService, BillService billService)
        {
            _repo = repo;
            _availabilityService = doctorAvailabilityService;
            _connectionString = options.Value.DefaultConnection;
            _doctorService = doctorService;
            _billService = billService;
        }
        public async Task<Result<AppointmentInfoDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<AppointmentInfoDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetFullAppointmentInfoByIDAsync(id);
        }
        public async Task<Result<int>> AddNewAppointmentAsync(AddNewAppointmentRequestDTO addNewAppointmentRequestDTO)
        {
            Result<bool> checkResult = await _repo.HasPenddingAppointment(addNewAppointmentRequestDTO.PatientId);
            if (!checkResult.Success)
                return new Result<int>(false, checkResult.Message, checkResult.ErrorCode);
            if (checkResult.Data)
                return new Result<int>(false, "This patient are pendding appointment, Not allowed", -1, 400);


            Result<List<AvailabilityInfoDTO>> availabilityResult = await _availabilityService.GetAllDoctorAvailabiltiesAsync(addNewAppointmentRequestDTO.DoctorId);
            if (!availabilityResult.Success)
                return new Result<int>(false, availabilityResult.Message, -1, availabilityResult.ErrorCode);

            DayOfWeek requestedDay = addNewAppointmentRequestDTO.Date.DayOfWeek;
            TimeOnly requestedTime = TimeOnly.FromDateTime(addNewAppointmentRequestDTO.Date);


            bool isValid = availabilityResult.Data.Any(a =>
                     a.DayOfWeek == requestedDay &&
                     requestedTime >= a.StartTime &&
                     requestedTime < a.EndTime);

            if (!isValid)
                return new Result<int>(false, "Outside doctor's availabilities", -1, 400);

            Result<decimal> feeResult = await _doctorService.GetConsultationFee(addNewAppointmentRequestDTO.DoctorId);
            if (!feeResult.Success)
                return new Result<int>(false, feeResult.Message, -1, feeResult.ErrorCode);


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    Result<int> CreateBillResult = await _billService.AddNewBillAsync(feeResult.Data, conn, tran);
                    if (!CreateBillResult.Success)
                    {
                        tran?.Rollback();
                        return new Result<int>(false, CreateBillResult.Message, -1, CreateBillResult.ErrorCode);
                    }

                    AddNewAppointmentDTO addNew = new AddNewAppointmentDTO(addNewAppointmentRequestDTO, feeResult.Data, CreateBillResult.Data);

                    Result<int> createAppointmentResult =  await _repo.AddNewAppointmentAsync(addNew, conn, tran);
                    if (!createAppointmentResult.Success)
                    {
                        tran?.Rollback();
                        return new Result<int>(false, createAppointmentResult.Message, -1, createAppointmentResult.ErrorCode);
                    }

                    tran.Commit();
                    return createAppointmentResult;

                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }
            }

            
        }
        public async Task<Result<bool>> UpdateAppointmentAsync(UpdateAppointmentDTO updateAppointmentDTO)
        {
            Result<AppointmentDTO> appointmentResult = await _repo.GetAppointmentInfoById(updateAppointmentDTO.Id);
            if (!appointmentResult.Success)
                return new Result<bool>(false, appointmentResult.Message, false, appointmentResult.ErrorCode);
            if (appointmentResult.Data.Status == AppointmentStatus.Cancelled || appointmentResult.Data.Status == AppointmentStatus.Completed)
                return new Result<bool>(false, "Can't update completed or cancelled appointment!", false, 400);

            return await _repo.UpdateAppointmentAsync(updateAppointmentDTO);
        }
        public async Task<Result<bool>> DeleteAppointmentAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteAppointmentAsync(id);
        }
        public async Task<Result<bool>> Cancel(int id)
        {
            return await _repo.Cancel(id);
        }
        public async Task<Result<bool>> ChangeStatus(int billId, AppointmentStatus status, SqlConnection conn, SqlTransaction tran)
        {
            return await _repo.ChangeStatus(billId, status, conn, tran);
        }
        public async Task<Result<bool>> Reschedule(RescheduleDTO rescheduleDTO)
        {
            if (rescheduleDTO.NewDate < DateTime.Now)
                return new Result<bool>(false, "Can't select date in the past!", false, 400);

            Result<AppointmentDTO> appointmentResult = await _repo.GetAppointmentInfoById(rescheduleDTO.Id);
            if (!appointmentResult.Success)
                return new Result<bool>(false, appointmentResult.Message, false, appointmentResult.ErrorCode);
            Result<List<AvailabilityInfoDTO>> availabilityResult = await _availabilityService.GetAllDoctorAvailabiltiesAsync(appointmentResult.Data.DoctorId);
            if (!availabilityResult.Success)
                return new Result<bool>(false, availabilityResult.Message, false, availabilityResult.ErrorCode);

            DayOfWeek requestedDay = rescheduleDTO.NewDate.DayOfWeek;
            TimeOnly requestedTime = TimeOnly.FromDateTime(rescheduleDTO.NewDate);


            bool isValid = availabilityResult.Data.Any(a =>
                     a.DayOfWeek == requestedDay &&
                     requestedTime >= a.StartTime &&
                     requestedTime < a.EndTime);
            if (!isValid)
                return new Result<bool>(false, "Outside doctor's availabilities", false, 400);


            return await _repo.Reschedule(rescheduleDTO);
        }
        public async Task<Result<List<AppointmentInfoDTO>>> GetAllAppointments(AppointmentFilterDTO filter)
        {
            return await _repo.GetAllAppointment(filter);
        }
        public async Task<Result<bool>> IsValidAsync(int id)
        {
            return await _repo.IsValidAsync(id);
        }
    }
}
