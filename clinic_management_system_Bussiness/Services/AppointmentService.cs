using Azure.Core;
using clinic_management_system_Bussiness.Services;
using clinic_management_system_DataAccess;
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

        public AppointmentService(AppointmentRepository repo, DoctorAvailabilityService doctorAvailabilityService)
        {
            _repo = repo;
            _availabilityService = doctorAvailabilityService;
        }
         public async Task<Result<AppointmentInfoDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<AppointmentInfoDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetFullAppointmentInfoByIDAsync(id); 
        }
        public async Task<Result<int>> AddNewAppointmentAsync(AddNewAppointmentDTO addNewAppointmentDTO)
        {
            Result<bool> checkResult = await _repo.HasPenddingAppointment(addNewAppointmentDTO.PatientId);
            if (!checkResult.success)
                return new Result<int>(false, checkResult.message, checkResult.errorCode);
            if (checkResult.data)
                return new Result<int>(false, "This patient are pendding appointment, not allowed", -1, 400);


            Result<List<AvailabilityInfoDTO>> availabilityResult = await _availabilityService.GetAllDoctorAvailabiltiesAsync(addNewAppointmentDTO.DoctorId);
            if (!availabilityResult.success)
                return new Result<int>(false, availabilityResult.message, -1, availabilityResult.errorCode);

            DayOfWeek requestedDay = addNewAppointmentDTO.Date.DayOfWeek;
            TimeOnly requestedTime = TimeOnly.FromDateTime(addNewAppointmentDTO.Date);


            bool isValid = availabilityResult.data.Any(a =>
                     a.DayOfWeek == requestedDay &&
                     requestedTime >= a.StartTime &&
                     requestedTime < a.EndTime);

            if (!isValid)
                return new Result<int>(false, "Outside doctor's availabilities", -1, 400);


            return await _repo.AddNewAppointmentAsync(addNewAppointmentDTO);
        }
        public async Task<Result<int>> UpdateAppointmentAsync(UpdateAppointmentDTO updateAppointmentDTO)
        {
            return await _repo.UpdateAppointmentAsync(updateAppointmentDTO);
        }
        public  async Task<Result<bool>> DeleteAppointmentAsync(int id)
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
        public async Task<Result<bool>> ChangeStatus(int id, AppointmentStatus status)
        {
            return await _repo.ChangeStatus(id, status);
        }
        public async Task<Result<bool>> Reschedule(RescheduleDTO rescheduleDTO)
        {
            if (rescheduleDTO.NewDate < DateTime.Now)
                return new Result<bool>(false, "Can't select date in the past!", false, 400);

            Result<AppointmentDTO> appointmentResult = await _repo.GetAppointmentInfoById(rescheduleDTO.Id);
            if (!appointmentResult.success)
                return new Result<bool>(false, appointmentResult.message, false, appointmentResult.errorCode);
            Result<List<AvailabilityInfoDTO>> availabilityResult = await _availabilityService.GetAllDoctorAvailabiltiesAsync(appointmentResult.data.DoctorId);
            if (!availabilityResult.success)
                return new Result<bool>(false, availabilityResult.message, false, availabilityResult.errorCode);

            DayOfWeek requestedDay = rescheduleDTO.NewDate.DayOfWeek;
            TimeOnly requestedTime = TimeOnly.FromDateTime(rescheduleDTO.NewDate);


            bool isValid = availabilityResult.data.Any(a =>
                     a.DayOfWeek == requestedDay &&
                     requestedTime >= a.StartTime &&
                     requestedTime < a.EndTime );
            if (!isValid)
                return new Result<bool>(false, "Outside doctor's availabilities", false, 400);


            return await _repo.Reschedule(rescheduleDTO);
        }
        public async Task<Result<List<AppointmentInfoDTO>>> GetAllAppointments(AppointmentFilterDTO filter)
        {
            return await _repo.GetAllAppointment(filter);
        }
    }
}
