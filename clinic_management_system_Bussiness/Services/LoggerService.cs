using Azure.Core;
using Microsoft.Data.SqlClient;
using SharedClasses;
using SharedClasses.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness
{
    public class LoggerService
    {
        private readonly AuditLogService _databaseLoggerService;
        public LoggerService(AuditLogService dataBaseLoggerService)
        {
            _databaseLoggerService = dataBaseLoggerService;
        }
        public async Task<Result<bool>> log(CreateAuditLogDTO createAuditLogDTO)
        {
            return await _databaseLoggerService._AddNewAuditLogAsync(createAuditLogDTO);
        }
    }
}
