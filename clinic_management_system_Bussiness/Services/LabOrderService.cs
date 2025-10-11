using SharedClasses;
using clinic_management_system_DataAccess;
namespace clinic_management_system_Bussiness
{
    public class LabOrderService
    {

        private readonly LabOrderRepository _repo;
        private readonly AppointmentRepository _appointmentRepo;


        public LabOrderService(LabOrderRepository repo, AppointmentRepository appointmentRepo)
        {
            _repo = repo;
            _appointmentRepo = appointmentRepo;
        }

        public async Task<Result<LabOrderDTO>> FindAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<LabOrderDTO>(false, "The request is invalid. Please check the input and try again.", null, 400);
            }
            return await _repo.GetLabOrderInfoByIDAsync(id);
        }

        public async Task<Result<int>> _AddNewLabOrderAsync(LabOrderDTO labOrderDTO)
        {
            if (labOrderDTO.appointmentId.HasValue)
            {
                Result<bool> checkResult = await _appointmentRepo.IsValidAsync((int)labOrderDTO.appointmentId);
                if (!checkResult.Success)
                    return new Result<int>(false, checkResult.Message, -1, checkResult.ErrorCode);
                if (!checkResult.Data)
                    return new Result<int>(false, "Can process with this appointemnt id!", -1, 400);
            }
            return await _repo.AddNewLabOrderAsync(labOrderDTO);
        }

        public async Task<Result<int>> _UpdateLabOrderAsync(LabOrderDTO labOrderDTO)
        {
            return await _repo.UpdateLabOrderAsync(labOrderDTO);
        }

        public async Task<Result<bool>> DeleteLabOrderAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteLabOrderAsync(id);
        }

    }
}
