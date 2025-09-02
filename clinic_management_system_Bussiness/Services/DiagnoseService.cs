using clinic_management_system_DataAccess;
using SharedClasses;
using SharedClasses.DTOS.Diagnose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness.Services
{
    public class DiagnoseService
    {
        private readonly DiagnoseRepository _repo;
        public DiagnoseService(DiagnoseRepository rep)
        {
            _repo = rep;
        }
        public async Task<Result<DiagnoseInfoDTO>> FindAsync(int id)
        {
            
            return await _repo.GetDiagnoseInfoByIDAsync(id);
        }

        public async Task<Result<int>> AddNewDiagnoseAsync(AddNewDiagnoseDTO addnew)
        {
            Result<bool> existResult = await _repo.IsExist(addnew.AppointmentId);
            if (!existResult.success)
                return new Result<int>(false, existResult.message, -1, existResult.errorCode);
            if (existResult.data)
                return new Result<int>(false, "This appointment already has diagnose!", -1, 400);

            return await _repo.AddNewDiagnoseAsync(addnew);
        }

        public async Task<Result<int>> UpdateDiagnoseAsync(UpdateDiagnoseDTO update)
        {
            return await _repo.UpdateDiagnoseAsync(update);
        }

        public async Task<Result<bool>> DeleteDiagnoseAsync(int id)
        {
            return await _repo.DeleteDiagnoseAsync(id);
        }
    }
}
