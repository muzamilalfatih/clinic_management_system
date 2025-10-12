using clinic_management_system_DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
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
        private readonly string _connectionString;
        private readonly LabOrderTestService _labOrderTestSevice;

        public LabOrderResultService(LabOrderResultRepository repo, IOptions<DatabaseSettings> options
            ,LabOrderTestService labOrderTestService)
        {
            _repo = repo;
            _connectionString = options.Value.DefaultConnection;
            _labOrderTestSevice = labOrderTestService;
        }
        public async Task<Result<LabOrderResultDTO>> FindAsync(int id)
        {
            return await _repo.GetByIDAsync(id);
        }
        public async Task<Result<List<LabOrderResultDTO>>> GetAllAsync(int labOrderTestId)
        {

            return await _repo.GetAllAsync(labOrderTestId);
        }

        public async Task<Result<bool>> AddNewAsync(AddNewLabOrderResultRequestDTO requestDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    Result<bool> orderResult = await _repo.AddNewAsync(requestDTO, conn, tran);
                    if (!orderResult.Success)
                    {
                        tran?.Rollback();
                        return orderResult;
                    }

                    Result<bool> completeOderTestResut = await _labOrderTestSevice.Complete(requestDTO.LabOderTestId, conn, tran);
                    if (!completeOderTestResut.Success)
                    {
                        tran?.Rollback();
                        return new Result<bool>(false, completeOderTestResut.Message, false, completeOderTestResut.ErrorCode);
                    }

                    tran.Commit();
                    return orderResult;


                }
                catch (Exception ex)
                {
                    tran?.Rollback();
                    return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                }
            }
        }
        public async Task<Result<bool>> UpdateAsync(UpdateLabOrderResultDTO updateDTO)
        {
            return await _repo.UpdateAsync(updateDTO);
        }
    }
}
