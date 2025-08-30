using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class AppointmentService
    {

        private readonly AppointmentRepository _repo;

        public AppointmentService(AppointmentRepository repo)
        {
            _repo = repo;
        }
         public async Task<Result<AppointmentDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<AppointmentDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetAppointmentInfoByIDAsync(id); 
        }

        public async Task<Result<int>> _AddNewAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            return await _repo.AddNewAppointmentAsync(appointmentDTO);
        }

        public async Task<Result<int>> _UpdateAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            return await _repo.UpdateAppointmentAsync( appointmentDTO);
        }

        public  async Task<Result<bool>> DeleteAppointmentAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteAppointmentAsync(id);
        }
    }
}
