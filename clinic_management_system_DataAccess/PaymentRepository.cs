using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class PaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<PaymentDTO>> GetPaymentInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Payments WHERE Id = @id";
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
                                PaymentDTO paymentDTO = new PaymentDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("BillId")),
                                     Convert.ToSingle(reader.GetDecimal(reader.GetOrdinal("PaidAmount"))),
                                     reader.GetInt32(reader.GetOrdinal("PaymentMethodId")),
                                     reader.GetDateTime(reader.GetOrdinal("Date"))
                                 );
                                return new Result<PaymentDTO>(true, "Payment found successfully", paymentDTO);
                            }
                            else
                            {
                                return new Result<PaymentDTO>(false, "Payment not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PaymentDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> AddNewPaymentAsync(PaymentDTO paymentDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO Payments
      (
      BillId
      ,PaidAmount
      ,PaymentMethodId
      ,Date)
VALUES
      (
      @BillId
      ,@PaidAmount
      ,@PaymentMethodId
      ,@Date);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BillId", paymentDTO.billId);
                    command.Parameters.AddWithValue("@PaidAmount", paymentDTO.paidAmount);
                    command.Parameters.AddWithValue("@PaymentMethodId", paymentDTO.paymentMethodId);
                    command.Parameters.AddWithValue("@Date", paymentDTO.date);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Payment added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add payment.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> UpdatePaymentAsync(PaymentDTO paymentDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Payments
SET 
    BillId = @BillId,
    PaidAmount = @PaidAmount,
    PaymentMethodId = @PaymentMethodId,
    Date = @Date
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", paymentDTO.id);
                    command.Parameters.AddWithValue("@BillId", paymentDTO.billId);
                    command.Parameters.AddWithValue("@PaidAmount", paymentDTO.paidAmount);
                    command.Parameters.AddWithValue("@PaymentMethodId", paymentDTO.paymentMethodId);
                    command.Parameters.AddWithValue("@Date", paymentDTO.date);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "Payment updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update payment.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeletePaymentAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Payments WHERE Id = @id";
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
                            return new Result<bool>(true, "Payment deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete payment.", false);
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
