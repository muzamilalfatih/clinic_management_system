using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Payment;
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
        public  async Task<Result<PaymentInfoDTO>> GetPaymentInfoByIDAsync(int billId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Payments.Id, Payments.BillId, Payments.PaidAmount, PaymentMethods.Name as PaymentMethod, Payments.Date
FROM     Payments INNER JOIN
                  PaymentMethods ON Payments.PaymentMethodId = PaymentMethods.Id
WHERE BillId = @BillId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BillId", billId);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                PaymentInfoDTO paymentDTO = new PaymentInfoDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("BillId")),
                                     reader.GetDecimal(reader.GetOrdinal("PaidAmount")),
                                     reader.GetString(reader.GetOrdinal("PaymentMethod")),
                                     reader.GetDateTime(reader.GetOrdinal("Date"))
                                 );
                                return new Result<PaymentInfoDTO>(true, "Payment found successfully", paymentDTO);
                            }
                            else
                            {
                                return new Result<PaymentInfoDTO>(false, "Payment not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PaymentInfoDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> AddNewPaymentAsync(AddNewPaymentDTO addNew, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO Payments
      (
      BillId
      ,PaidAmount
      ,PaymentMethodId)
VALUES
      (
      @BillId
      ,@PaidAmount
      ,@PaymentMethodId);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@BillId", addNew.BillId);
                command.Parameters.AddWithValue("@PaidAmount", addNew.PaidAmont);
                command.Parameters.AddWithValue("@PaymentMethodId", addNew.PaymentMethodId);


                object result = await command.ExecuteScalarAsync();
                int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (id > 0)
                {
                    return new Result<bool>(true, "Payment added successfully.", true);
                }
                else
                {
                    return new Result<bool>(false, "Failed to add payment.", false);
                }

            }
        }
    }
}
