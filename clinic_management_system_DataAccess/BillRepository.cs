using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Bills;
using SharedClasses.Enums;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
namespace clinic_management_system_DataAccess
{
    public class BillRepository
    {
        private readonly string _connectionString;

        public BillRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }

        public  async Task<Result<BillInfoDTO>> GetBillInfoByIDAsync(int id)
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
                                BillInfoDTO billDTO = new BillInfoDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetDecimal(reader.GetOrdinal("Amount")),
                                     reader.GetDateTime(reader.GetOrdinal("date")),
                                     (BillStatus)reader.GetByte(reader.GetOrdinal("Status"))
                                 );
                                return new Result<BillInfoDTO>(true, "Bill found successfully", billDTO);
                            }
                            else
                            {
                                return new Result<BillInfoDTO>(false, "Bill not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<BillInfoDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public  async Task<Result<int>> AddNewBillAsync(decimal amount, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO Bills
      (
      Amount)
VALUES
      (
      @Amount);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@Amount", amount);


                object? result = await command.ExecuteScalarAsync();
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
                        int rowAffected = await command.ExecuteNonQueryAsync();

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
        public async Task<Result<bool>> Pay(int id, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
                                Update Bills
                                SET Status = 2
                                WHERE Id = @Id";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@Id", id);

                int rowAffected = await command.ExecuteNonQueryAsync();

                if (rowAffected > 0)
                {
                    return new Result<bool>(true, "Bill paid successfully.", true);
                }
                else
                {
                    return new Result<bool>(false, "Failed to pay bill.", false);
                }

            }
        }

    }
}
