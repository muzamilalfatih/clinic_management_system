using clinic_management_system_DataAccess;
using SharedClasses;
using SharedClasses.DTOS.LabDevices;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness.Services
{
    public class LabDeviceService
    {
        private readonly LabDeviceRepository _repo;

        public LabDeviceService(LabDeviceRepository repo)
        {
            _repo = repo;
        }
        public async Task<Result<LabDeviceDTO>> FindAsync(int id)
        {
            return await _repo.GetLabDeviceAsync(id);
        }
        public async Task<Result<LabDeviceDTO>> FindAsync(string name)
        {
            return await _repo.GetLabDeviceAsync(name);
        }
        public async Task<Result<List<LabDeviceDTO>>> GetAllAsync(int pageNumbe, int pageSize)
        {
            pageSize = (pageSize > 20) ? 20 : pageSize;

            return await _repo.GetAllAsync(pageNumbe, pageSize);
        }
        public async Task<Result<int>> AddNewAsync(CreateLabDeviceDTO newLabDevice)
        {
            return await _repo.AddNewLabDeviceAsync(newLabDevice);
        }
        public async Task<Result<bool>> UpdateAsync(UpdateLabDeviceDTO updateDTO)
        {
            return await _repo.UpdateDoctorAsync(updateDTO);
        }
        public async Task<Result<int>> GetIdAsync(string name)
        {
            return await _repo.GetIdAsync(name);
        }
    }
}
