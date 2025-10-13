using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.LabOrderTests;
using SharedClasses.Enums;
using System.Data;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
namespace clinic_management_system_DataAccess
{
    public class LabOrderTestRepository
    {
        private readonly string _connectionString;

        public LabOrderTestRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<LabOrderTestDTO>> GetLabOrderTestInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM LabOrderTests WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                LabOrderTestDTO labOrderTestDTO = new LabOrderTestDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("LabOrderId")),
                                     reader.GetInt32(reader.GetOrdinal("LabTestId")),
                                     reader.GetDecimal(reader.GetOrdinal("Fee")),
                                      (LabOrderTestStatus)reader.GetByte(reader.GetOrdinal("Status"))
                                 );
                                return new Result<LabOrderTestDTO>(true, "Lab Order Test found successfully", labOrderTestDTO);
                            }
                            else
                            {
                                return new Result<LabOrderTestDTO>(false, "Lab Order Test not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabOrderTestDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        //        public async Task<Result<int>> AddNewLabOrderTestAsync(AddNewLabOrderTestDTO createLabOrderTestDTO)
        //        {
        //            using (SqlConnection connection = new SqlConnection(_connectionString))
        //            {
        //                string query = @"
        //INSERT INTO LabOrderTests
        //      (
        //      LabOrderId
        //      ,LabTestId
        //      ,NormalRange
        //      ,Unit)
        //VALUES
        //      (
        //      @LabOrderId
        //      ,@LabTestId
        //      ,@NormalRange
        //      ,@Unit);
        //SELECT SCOPE_IDENTITY();
        //";
        //                using (SqlCommand command = new SqlCommand(query, connection))
        //                {
        //                    command.Parameters.AddWithValue("@LabOrderId", createLabOrderTestDTO.labOrderId);
        //                    command.Parameters.AddWithValue("@LabTestId", createLabOrderTestDTO.labTestId);
        //                    command.Parameters.AddWithValue("@NormalRange", createLabOrderTestDTO.normalRange);
        //                    command.Parameters.AddWithValue("@Unit", createLabOrderTestDTO.unit);


        //                    try
        //                    {
        //                        await connection.OpenAsync();
        //                        object result = await command.ExecuteScalarAsync();
        //                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
        //                        if (id > 0)
        //                        {
        //                            return new Result<int>(true, "LabOrderTest added successfully.", id);
        //                        }
        //                        else
        //                        {
        //                            return new Result<int>(false, "Failed to add labOrderTest.", -1);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
        //                    }

        //                }
        //            }
        //        }

        public async Task<Result<bool>> AddNewLabOrderTestsAsync(List<AddNewLabOrderTestDTO> tests, int labOrderId, SqlConnection
            conn, SqlTransaction tran)
        {
            if (tests == null || tests.Count == 0)
                return new Result<bool>(false, "No data provided!", false, 400);

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO LabOrderTests (LabOrderId, LabTestId, Fee) VALUES ");

            var parameters = new List<SqlParameter>();
            for (int i = 0; i < tests.Count; i++)
            {
                AddNewLabOrderTestDTO orderTest = tests[i];

                string valuesClause = $"(@LabOrderId, @LabTestId{i}, @Fee{i})";

                if (i > 0)
                    queryBuilder.Append(", ");
                queryBuilder.Append(valuesClause);

                parameters.Add(new SqlParameter($"@LabTestId{i}", SqlDbType.Int) { Value = orderTest.LabTestId });
                parameters.Add(new SqlParameter($"@Fee{i}", SqlDbType.Decimal, 100) { Value = orderTest.Fee });
            }
            queryBuilder.AppendLine(";SELECT SCOPE_IDENTITY()");
            // One shared LabOrderId param
            parameters.Add(new SqlParameter("@LabOrderId", SqlDbType.Int) { Value = labOrderId });

            using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), conn, tran))
            {
                command.Parameters.AddRange(parameters.ToArray());
                object? result = await command.ExecuteScalarAsync();
                bool success = result != DBNull.Value ? Convert.ToInt32(result) > 0 : false;

                return new Result<bool>(true, "Lab order tests inserted successfully!", success);
            }
        }

        public async Task<Result<bool>> Complete(int Id, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"UPDATE LabOrderTests
                                    SET 
                                        Status = 2
                                    WHERE Id = @Id 
                                    select @@ROWCOUNT";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@Id", Id);


                object? result = await command.ExecuteScalarAsync();
                int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (rowAffected > 0)
                {
                    return new Result<bool>(true, "Lab order result  Cancelled.", true);
                }
                else
                {
                    return new Result<bool>(false, "un expected error on the server .", false, 500);
                }

            }
        }
        public async Task<Result<bool>> DeleteLabOrderTestAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM LabOrderTests WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "LabOrderTest deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete labOrderTest.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> HadPenddingTestAsync(int labOrderId, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
select top 1 Id from LabOrderTests
where labOrderId = @LabOrderId and status = 1";

            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@LabOrderId", labOrderId);
                bool isFound;
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    isFound = reader.HasRows;
                }
                return new Result<bool>(true, "Check completed.", isFound);

            }

        }
        public async Task<Result<int>> GetLabOrderIdAsync(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT LabOrderId FROM LabOrderTests
WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Lab order  id retrieved successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Lab order test not found.", -1, 404);
                        }

                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> IsFirstTestAsync(int labOrderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select top 1 1 from LabOrderTests
where labOrderId = @LabOrderId and  Status = @Status
";
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LabOrderId", labOrderId);
                        command.Parameters.AddWithValue("@Status", (int) LabOrderTestStatus.Completed);
                        bool isFound;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            isFound = reader.HasRows;
                        }
                        return new Result<bool>(true, "Check completed.", !isFound);

                    }
                }
                catch(Exception ex)
                {
                    return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                }
            }
        }
        public async Task<Result<LabOrderTestStatus>> GetStatusAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Status FROM LabOrderTests
WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int statusCode = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (statusCode > 0)
                        {
                            return new Result<LabOrderTestStatus>(true, "Lab order  id retrieved successfully.",(LabOrderTestStatus) statusCode);
                        }
                        else
                        {
                            return new Result<LabOrderTestStatus>(false, "Lab order test not found.", default, 404);
                        }

                    }
                    catch (Exception ex)
                    {
                        return new Result<LabOrderTestStatus>(false, "An unexpected error occurred on the server.", default, 500);
                    }

                }
            }
        }
    }
}
