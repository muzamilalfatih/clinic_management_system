using SharedClasses;
using clinic_management_system_DataAccess;
using SharedClasses.DTOS;
using Microsoft.Data.SqlClient;
namespace clinic_management_system_Bussiness
{
    public class AuditLogService
    {
        private readonly AuditLogRepository _repo;

        public AuditLogService(AuditLogRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<AuditLogDTO>> FindAsync(int id)
        {
            return await _repo.GetAuditLogInfoByIDAsync(id);
        }

        public async Task<Result<bool>> _AddNewAuditLogAsync(CreateAuditLogDTO createAuditLogDTO)
        {
            return await _repo.AddNewAuditLogAsync(createAuditLogDTO);
        }

        public async Task<Result<int>> _UpdateAuditLogAsync(AuditLogDTO auditLogDTO)
        {
            return await _repo.UpdateAuditLogAsync(auditLogDTO);
        }

        public async Task<Result<bool>> DeleteAuditLogAsync(int id)
        {
            if (id <= 0)
            {
                return new Result<bool>(false, "The request is invalid. Please check the input and try again.", false, 400);
            }
            return await _repo.DeleteAuditLogAsync(id);
        }
    }
}
