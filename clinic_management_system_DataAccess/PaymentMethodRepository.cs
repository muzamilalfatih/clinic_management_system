using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class PaymentMethodRepository
    {
        private readonly string _connectionString;

        public PaymentMethodRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<PaymentMethodDTO>> GetPaymentMethodInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM PaymentMethods WHERE Id = @id";
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
                                PaymentMethodDTO paymentMethodDTO = new PaymentMethodDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetString(reader.GetOrdinal("Name"))
                                 );
                                return new Result<PaymentMethodDTO>(true, "PaymentMethod found successfully", paymentMethodDTO);
                            }
                            else
                            {
                                return new Result<PaymentMethodDTO>(false, "PaymentMethod not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PaymentMethodDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> AddNewPaymentMethodAsync(PaymentMethodDTO paymentMethodDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO PaymentMethods
      (
      Name)
VALUES
      (
      @Name);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", paymentMethodDTO.name);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "PaymentMethod added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add paymentMethod.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> UpdatePaymentMethodAsync(PaymentMethodDTO paymentMethodDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE PaymentMethods
SET 
    Name = @Name
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", paymentMethodDTO.id);
                    command.Parameters.AddWithValue("@Name", paymentMethodDTO.name);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "PaymentMethod updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update paymentMethod.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeletePaymentMethodAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM PaymentMethods WHERE Id = @id";
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
                            return new Result<bool>(true, "PaymentMethod deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete paymentMethod.", false);
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
