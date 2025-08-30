using clinic_management_system_DataAccess;
using SharedClasses;
using SharedClasses.DTOS.DoctorAvailability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness.Services
{
    public class DoctorAvailabilityService
    {
        private readonly DoctorAvailabilityRepository _repo;
        public DoctorAvailabilityService(DoctorAvailabilityRepository repo)
        {
            _repo = repo;
        }
        public async Task<Result<List<AvailabilityInfoDTO>>> GetAllDoctorAvailabiltiesAsync(int doctorId)
        {
            return await _repo.GetAllDoctorAvailabiltiesAsync(doctorId);
        }
        public async Task<Result<bool>> CreateAvailabiltiesAsync(int usreId, List<CreateAvailabilityRequestDTO> createRequestDTO)
        {
            List<CreateAvailabilityDTO> createAvailabilities = new List<CreateAvailabilityDTO>();
            foreach(var item in createRequestDTO)
            {
                createAvailabilities.Add( new CreateAvailabilityDTO(usreId, item));
            }
            return await _repo.CreateAvailabiltiesAsync(createAvailabilities);
        }
        public async Task<Result<bool>> UpdateAvailabilityAsync(UpdateDoctorAvialbilityDTO updateDoctorAvialbilityDTO)
        {
            return await _repo.UpdateAvailabilityAsync(updateDoctorAvialbilityDTO);
        }
        public async Task<Result<bool>> DeleteAvailabilityAsync(int id)
        {
            return await _repo.DeleteAvailabilityAsync(id);
        }
    }
}
