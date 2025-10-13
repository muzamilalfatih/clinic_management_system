using Azure.Core;
using clinic_management_system_DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using SharedClasses;
using SharedClasses.DTOS.LabOrderResults;
using SharedClasses.Enums;
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
        private readonly LabOrderTestService _labOrderTestService;
        private readonly LabOrderService _labOrderService;

        public LabOrderResultService(LabOrderResultRepository repo, IOptions<DatabaseSettings> options
            ,LabOrderTestService labOrderTestService, LabOrderService labOrderService)
        {
            _repo = repo;
            _connectionString = options.Value.DefaultConnection;
            _labOrderTestService = labOrderTestService;
            _labOrderService = labOrderService;
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

            Result<int> LabOrderIdResult = await _labOrderTestService.GetLabOrderId(requestDTO.LabOderTestId);
            if (!LabOrderIdResult.Success)
                return new Result<bool>(false, LabOrderIdResult.Message, false, LabOrderIdResult.ErrorCode);
            Result<bool> isFirstResult = await _labOrderTestService.IsFirstTestAsync(LabOrderIdResult.Data);
            if (!isFirstResult.Data)
            {
                return new Result<bool>(false, isFirstResult.Message, false, isFirstResult.ErrorCode);
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlTransaction? tran = null;
                try
                {
                    await conn.OpenAsync();
                    tran = conn.BeginTransaction();

                    if (isFirstResult.Data)
                    {
                        Result<bool> inProgressLabResult = await _labOrderService.ChangeStatus(LabOrderIdResult.Data, 
                            LabOrderStatus.InProgress, conn, tran);
                        if (!inProgressLabResult.Success)
                        {
                            tran.Rollback();
                            return new Result<bool>(false, inProgressLabResult.Message, false, inProgressLabResult.ErrorCode);
                        }
                    }
                    Result<bool> orderResult = await _repo.AddNewAsync(requestDTO, conn, tran);
                    if (!orderResult.Success)
                    {
                        tran?.Rollback();
                        return orderResult;
                    }

                    Result<bool> completeOderTestResut = await _labOrderTestService.Complete(requestDTO.LabOderTestId, conn, tran);
                    if (!completeOderTestResut.Success)
                    {
                        tran?.Rollback();
                        return new Result<bool>(false, completeOderTestResut.Message, false, completeOderTestResut.ErrorCode);
                    }

                    Result<bool> hasPenddingTestsResult = await _labOrderTestService.HasPeddingTestAsync(LabOrderIdResult.Data, conn, tran);
                    if (!hasPenddingTestsResult.Success)
                    {
                        tran.Rollback();
                        return new Result<bool>(false, hasPenddingTestsResult.Message, false, hasPenddingTestsResult.ErrorCode);
                    }
                    if (!hasPenddingTestsResult.Data)
                    {
                        Result<bool> completeLabOrderResult = await _labOrderService.ChangeStatus(LabOrderIdResult.Data, LabOrderStatus.Completed, conn, tran);
                        if (!completeLabOrderResult.Success)
                        {
                            tran.Rollback();
                            return new Result<bool>(false, completeLabOrderResult.Message, false, completeLabOrderResult.ErrorCode);
                        }
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
