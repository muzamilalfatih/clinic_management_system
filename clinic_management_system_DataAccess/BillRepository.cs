using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using System.Data;
using System.Diagnostics;
namespace clinic_management_system_DataAccess
{
    public class BillRepository
    {
        private readonly string _connectionString;

        public BillRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }

        public  async Task<Result<BillDTO>> GetBillInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Bills WHERE Id = @id";
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
                                BillDTO billDTO = new BillDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     Convert.ToSingle(reader.GetDecimal(reader.GetOrdinal("TotalAmount"))),
                                     reader.GetDateTime(reader.GetOrdinal("date")),
                                     reader.GetByte(reader.GetOrdinal("Status"))
                                 );
                                return new Result<BillDTO>(true, "Bill found successfully", billDTO);
                            }
                            else
                            {
                                return new Result<BillDTO>(false, "Bill not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<BillDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> AddNewBillAsync(BillDTO billDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO Bills
      (
      TotalAmount
      ,date
      ,Status)
VALUES
      (
      @TotalAmount
      ,@date
      ,@Status);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TotalAmount", billDTO.totalAmount);
                    command.Parameters.AddWithValue("@date", billDTO.date);
                    command.Parameters.AddWithValue("@Status", billDTO.status);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Bill added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add bill.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> UpdateBillAsync(BillDTO billDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Bills
SET 
    TotalAmount = @TotalAmount,
    date = @date,
    Status = @Status
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", billDTO.id);
                    command.Parameters.AddWithValue("@TotalAmount", billDTO.totalAmount);
                    command.Parameters.AddWithValue("@date", billDTO.date);
                    command.Parameters.AddWithValue("@Status", billDTO.status);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "Bill updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update bill.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeleteBillAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Bills WHERE Id = @id";
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
                            return new Result<bool>(true, "Bill deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete bill.", false);
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
