using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.LabTest;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text;
namespace clinic_management_system_DataAccess
{
    public class LabTestRepository
    {
        private readonly string _connectionString;

        public LabTestRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<LabTestDTO>> GetLabTestInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM LabTests WHERE Id = @id";
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
                                LabTestDTO labTestDTO = new LabTestDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetString(reader.GetOrdinal("Name")),
                                     reader.GetString(reader.GetOrdinal("Description")),
                                     reader.GetDecimal(reader.GetOrdinal("Price"))
                                 );
                                return new Result<LabTestDTO>(true, "LabTest found successfully", labTestDTO);
                            }
                            else
                            {
                                return new Result<LabTestDTO>(false, "LabTest not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabTestDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<int>> AddNewLabTestAsync(LabTestDTO labTestDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO LabTests
      (
      Name
      ,Description
      ,Price)
VALUES
      (
      @Name
      ,@Description
      ,@Price);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", labTestDTO.name);
                    command.Parameters.AddWithValue("@Description", labTestDTO.description);
                    command.Parameters.AddWithValue("@Price", labTestDTO.price);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "LabTest added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add labTest.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<int>> UpdateLabTestAsync(LabTestDTO labTestDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE LabTests
SET 
    Name = @Name,
    Description = @Description,
    Price = @Price
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", labTestDTO.id);
                    command.Parameters.AddWithValue("@Name", labTestDTO.name);
                    command.Parameters.AddWithValue("@Description", labTestDTO.description);
                    command.Parameters.AddWithValue("@Price", labTestDTO.price);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "LabTest updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update labTest.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteLabTestAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM LabTests WHERE Id = @id";
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
                            return new Result<bool>(true, "LabTest deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete labTest.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<float>> GetTotalPrice(List<int> labTestIds)
        {
            if (labTestIds == null || labTestIds.Count == 0)
                return new Result<float>(false, "No test IDs provided.", 0, 400);

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT SUM(Price) AS TotalPrice FROM LabTests WHERE Id IN (");

            var parameters = new List<SqlParameter>();
            for (int i = 0; i < labTestIds.Count; i++)
            {
                string paramName = $"@Id{i}";
                if (i > 0)
                    queryBuilder.Append(", ");
                queryBuilder.Append(paramName);
                parameters.Add(new SqlParameter(paramName, SqlDbType.Int) { Value = labTestIds[i] });
            }

            queryBuilder.Append(");");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        float sum = result != DBNull.Value ? Convert.ToSingle(result) : 0;

                        return new Result<float>(true, "Success", sum);
                    }
                    catch (Exception ex)
                    {
                        return new Result<float>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }

            }
        }
    }
}
