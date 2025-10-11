using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.LabOrderTests;
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
        public  async Task<Result<LabOrderTestDTO>> GetLabOrderTestInfoByIDAsync(int id)
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
                                     reader.GetString(reader.GetOrdinal("Result")),
                                     reader.GetString(reader.GetOrdinal("NormalRange")),
                                     reader.GetString(reader.GetOrdinal("Unit")),
                                     reader.GetDateTime(reader.GetOrdinal("ResultDate")),
                                     (LabOrderTestStatus)reader.GetByte(reader.GetOrdinal("Status")),
                                     reader.GetInt32(reader.GetOrdinal("TestedByTechnicianID"))
                                 );
                                return new Result<LabOrderTestDTO>(true, "LabOrderTest found successfully", labOrderTestDTO);
                            }
                            else
                            {
                                return new Result<LabOrderTestDTO>(false, "LabOrderTest not found.", null, 404);
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

        public  async Task<Result<int>> AddNewLabOrderTestAsync(CreateLabOrderTestDTO createLabOrderTestDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO LabOrderTests
      (
      LabOrderId
      ,LabTestId
      ,NormalRange
      ,Unit)
VALUES
      (
      @LabOrderId
      ,@LabTestId
      ,@NormalRange
      ,@Unit);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LabOrderId", createLabOrderTestDTO.labOrderId);
                    command.Parameters.AddWithValue("@LabTestId", createLabOrderTestDTO.labTestId);
                    command.Parameters.AddWithValue("@NormalRange", createLabOrderTestDTO.normalRange);
                    command.Parameters.AddWithValue("@Unit", createLabOrderTestDTO.unit);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "LabOrderTest added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add labOrderTest.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> AddNewLabOrderTestAsync(List<CreateLabOrderTestDTO> tests, SqlConnection 
            conn, SqlTransaction tran)
        {
            if (tests == null || tests.Count == 0)
                return new  Result<bool>(false, "No data provided!",false , 400);

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO LabOrderTests (LabOrderId, LabTestId, NormalRange, Unit) VALUES ");

            var parameters = new List<SqlParameter>();
            for (int i = 0; i < tests.Count; i++)
            {
                CreateLabOrderTestDTO test = tests[i];
                string valuesClause = $"(@LabOrderId, @LabTestId{i}, @NormalRange{i}, @Unit{i})";

                if (i > 0)
                    queryBuilder.Append(", ");
                queryBuilder.Append(valuesClause);

                parameters.Add(new SqlParameter($"@LabTestId{i}", SqlDbType.Int) { Value = test.labTestId });
                parameters.Add(new SqlParameter($"@NormalRange{i}", SqlDbType.NVarChar, 100) { Value = test.normalRange ?? (object)DBNull.Value });
                parameters.Add(new SqlParameter($"@Unit{i}", SqlDbType.NVarChar, 50) { Value = test.unit ?? (object)DBNull.Value });
            }
            queryBuilder.AppendLine(";SELECT SCOPE_IDENTITY()");
            // One shared LabOrderId param
            parameters.Add(new SqlParameter("@LabOrderId", SqlDbType.Int) { Value = tests[0].labOrderId });

            using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), conn, tran))
            {
                command.Parameters.AddRange(parameters.ToArray());
                try
                {
                    object result = await command.ExecuteScalarAsync();
                    bool success = result != DBNull.Value ? Convert.ToInt32(result) > 0 : false;

                    return new Result<bool>(true, "Lab order tests inserted successfully!", success);
                }
                catch (Exception ex)
                {
                    return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                }
            }
        }
        public  async Task<Result<int>> UpdateLabOrderTestAsync(LabOrderTestDTO labOrderTestDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE LabOrderTests
SET 
    LabOrderId = @LabOrderId,
    LabTestId = @LabTestId,
    Result = @Result,
    NormalRange = @NormalRange,
    Unit = @Unit,
    ResultDate = @ResultDate,
    Status = @Status,
    TestedByTechnicianID = @TestedByTechnicianID
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", labOrderTestDTO.id);
                    command.Parameters.AddWithValue("@LabOrderId", labOrderTestDTO.labOrderId);
                    command.Parameters.AddWithValue("@LabTestId", labOrderTestDTO.labTestId);
                    command.Parameters.AddWithValue("@Result", labOrderTestDTO.result);
                    command.Parameters.AddWithValue("@NormalRange", labOrderTestDTO.normalRange);
                    command.Parameters.AddWithValue("@Unit", labOrderTestDTO.unit);
                    command.Parameters.AddWithValue("@ResultDate", labOrderTestDTO.resultDate);
                    command.Parameters.AddWithValue("@Status", labOrderTestDTO.status);
                    command.Parameters.AddWithValue("@TestedByTechnicianID", labOrderTestDTO.testedByTechnicianID);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "LabOrderTest updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update labOrderTest.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeleteLabOrderTestAsync(int id)
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


    }
}
