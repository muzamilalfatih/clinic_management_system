using clinic_management_system_DataAccess;
using SharedClasses;
using SharedClasses.DTOS.LabOrderResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness.Services
{
    public class LabOrderResultService
    {
        private readonly LabOrderResultRepository _repo;

        public LabOrderResultService(LabOrderResultRepository repo)
        {
            _repo = repo;
        }
        public async Task<Result<LabOrderResultDTO>> FindAsync(int id)
        {
            return await _repo.GetByIDAsync(id);
        }
        public async Task<Result<List<LabOrderResultDTO>>> GetAllAsync(int labOrderTestId)
        {

            return await _repo.GetAllAsync(labOrderTestId);
        }

        public async Task<Result<bool>> AddNewAsync(List<AddNewOrderResultDTO> results)
        {
            return await _repo.AddNewAsync(results);
        }
        public async Task<Result<bool>> UpdateAsync(UpdateLabOrderResultDTO updateDTO)
        {
            return await _repo.UpdateAsync(updateDTO);
        }
    }
}
