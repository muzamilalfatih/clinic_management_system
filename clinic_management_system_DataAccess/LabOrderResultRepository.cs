using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.LabOrderResults;
using SharedClasses.DTOS.LabTestParameter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_DataAccess
{
    public class LabOrderResultRepository
    {
        private readonly string _connectionString;
        public LabOrderResultRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }

        private (string query, List<SqlParameter> parameters) _queryBuilder(AddNewLabOrderResultRequestDTO request)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO LabOrderNewResultDTOs (LabOrderTestId, LabTestParameterId, Result) VALUES ");

            var parameters = new List<SqlParameter>();
            for (int i = 0; i < request.NewOrderResults.Count; i++)
            {
                AddNewOrderResultDTO labTest = request.NewOrderResults[i];

                string valuesClause = $"(@LabOrderTestId, @LabTestParameterId{i}, @Result{i})";

                if (i > 0)
                    queryBuilder.Append(", ");
                queryBuilder.Append(valuesClause);

                parameters.Add(new SqlParameter($"@LabTestParameterId{i}", SqlDbType.Int) { Value = labTest.LabTestParameterId });
                parameters.Add(new SqlParameter($"@Result{i}", SqlDbType.NVarChar, 100) { Value = labTest.Result });
            }
            queryBuilder.AppendLine(";SELECT SCOPE_IDENTITY()");
            // One shared LabOrderId param
            parameters.Add(new SqlParameter("@LabOrderTestId", SqlDbType.Int) { Value = request.NewOrderResults });

            return (queryBuilder.ToString(), parameters);
        }
        public async Task<Result<LabOrderResultDTO>> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM LabOrderResults WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                LabOrderResultDTO resultDTO = new LabOrderResultDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("LabOrderTestId")),
                                     reader.GetInt32(reader.GetOrdinal("LabTestParameterId")),
                                     reader.GetString(reader.GetOrdinal("Result"))
                                 );
                                return new Result<LabOrderResultDTO>(true, "Result found successfully", resultDTO);
                            }
                            else
                            {
                                return new Result<LabOrderResultDTO>(false, "Result not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabOrderResultDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<List<LabOrderResultDTO>>> GetAllAsync(int labOrderTestId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM LabOrderResults WHERE LabOrderTestId = @LabOrderTestId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LabOrderTestId", labOrderTestId);

                    try
                    {
                        await connection.OpenAsync();
                        List<LabOrderResultDTO> resultsDTO = new List<LabOrderResultDTO>();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                resultsDTO.Add(new LabOrderResultDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("LabOrderTestId")),
                                     reader.GetInt32(reader.GetOrdinal("LabTestParameterId")),
                                     reader.GetString(reader.GetOrdinal("Result"))
                                 ));
                            }
                            if (resultsDTO.Count() > 0)
                                return new Result<List<LabOrderResultDTO>>(true, "Results found successfully", resultsDTO);
                            else
                            {
                                return new Result<List<LabOrderResultDTO>>(false, "Results not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<LabOrderResultDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> AddNewAsync(AddNewLabOrderResultRequestDTO request, SqlConnection conn, SqlTransaction tran)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (request == null || request.NewOrderResults.Count == 0)
                    return new Result<bool>(false, "No data provided!", false, 400);

                (string query, List<SqlParameter> parameters) = _queryBuilder(request);


                using (SqlCommand command = new SqlCommand(query.ToString()))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    object? result = await command.ExecuteScalarAsync();
                    bool success = result != DBNull.Value ? Convert.ToInt32(result) > 0 : false;

                    return new Result<bool>(true, "Lab order test results inserted successfully!", success);
                }
            }
        }
        public async Task<Result<bool>> UpdateAsync(UpdateLabOrderResultDTO UpdateDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE LabOrderResults
SET 
    LabOrderTestId = @LabOrderTestId,
    LabTestParameterId = @LabTestParameterId,
    Result = @Result
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", UpdateDTO.Id);
                    command.Parameters.AddWithValue("@LabOrderTestId", UpdateDTO.LabOrderTestId);
                    command.Parameters.AddWithValue("@LabTestParameterId", UpdateDTO.LabTestParameterId);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Result updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to update result.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
       
    }
}
