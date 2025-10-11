using clinic_management_system_DataAccess;
using SharedClasses;
using SharedClasses.DTOS.LabTestParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness.Services
{
    public class LabTestParameterService
    {
        private readonly LabTestParameterRepository _repo;
        public LabTestParameterService(LabTestParameterRepository rep)
        {
            _repo = rep;
        }
        public async Task<Result<LabTestParameterDTO>> FindAsync(int id)
        {

            return await _repo.GetByIDAsync(id);
        }
        public async Task<Result<List<LabTestParameterDTO>>> GetAllAsync(int testId)
        {

            return await _repo.GetAllAsync(testId);
        }

        public async Task<Result<int>> AddNewLabTestParameterAsync(AddNewLabTestParameterDTO addnew)
        {
            Result<bool> existenceResult = await _repo.IsExistAsync(addnew.LabTestId, addnew.Name);
            if (!existenceResult.Success)
                return new Result<int>(false, existenceResult.Message, -1, existenceResult.ErrorCode);
            if (existenceResult.Data)
                return new Result<int>(false, "This Lab test already has this parameter!", -1, 400);

            return await _repo.AddNewLabTestParameterAsync(addnew);
        }

        public async Task<Result<int>> UpdateLabTestParameterAsync(UpdateLabTestParameterDTO update)
        {
            Result<bool> existenceResult = await _repo.IsExistAsync(update.LabTestId, update.Name);
            if (!existenceResult.Success)
                return new Result<int>(false, existenceResult.Message, -1, existenceResult.ErrorCode);
            if (existenceResult.Data)
                return new Result<int>(false, "This Lab test already has this parameter!", -1, 400);

            return await _repo.UpdateLabTestParameterAsync(update);
        }

        public async Task<Result<bool>> DeleteLabTestParameterAsync(int id)
        {
            return await _repo.DeleteLabTestParameterAsync(id);
        }
    }
}
