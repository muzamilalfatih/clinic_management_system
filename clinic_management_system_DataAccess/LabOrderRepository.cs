using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.LabOrder;
using SharedClasses.Enums;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class LabOrderRepository
    {
        private readonly string _connectionString;

        public LabOrderRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<LabOrderDTO>> GetLabOrderInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM LabOrders WHERE Id = @id";
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
                                LabOrderDTO labOrderDTO = new LabOrderDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.IsDBNull(reader.GetOrdinal("AppointmentId"))
                                        ? null
                                        : reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                                     reader.IsDBNull(reader.GetOrdinal("PersonId"))
                                        ? null
                                        : reader.GetInt32(reader.GetOrdinal("PersonId")),
                                      reader.IsDBNull(reader.GetOrdinal("TestedByTechnicianId"))
                                        ? null
                                        : reader.GetInt32(reader.GetOrdinal("TestedByTechnicianId")),
                                     reader.GetInt32(reader.GetOrdinal("BillId")),
                                     reader.GetDateTime(reader.GetOrdinal("Date")),
                                     (LabOrderStatus)reader.GetByte(reader.GetOrdinal("Status")),
                                     reader.IsDBNull(reader.GetOrdinal("Notes"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("Notes"))
                                 );
                                return new Result<LabOrderDTO>(true, "LabOrder found successfully", labOrderDTO);
                            }
                            else
                            {
                                return new Result<LabOrderDTO>(false, "LabOrder not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabOrderDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> AddNewLabOrderAsync(AddNewLabOrderDTO addnew, int billId, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO LabOrders
      (
      AppointmentId
      ,PersonId
      ,Notes
      ,BillId)
VALUES
      (
      @AppointmentId
      ,@PersonId
      ,@Notes
      ,@BillId);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@AppointmentId", (object?)addnew.AppointmentId ?? DBNull.Value);
                command.Parameters.AddWithValue("@PersonId", (object?)addnew.PersonId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Notes", (object?)addnew.Notes ?? DBNull.Value);
                command.Parameters.AddWithValue("@BillId", billId);

                object result = await command.ExecuteScalarAsync();
                int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (id > 0)
                {
                    return new Result<int>(true, "LabOrder added successfully.", id);
                }
                else
                {
                    return new Result<int>(false, "Failed to add labOrder.", -1);
                }
            }
        }
        public  async Task<Result<int>> UpdateLabOrderAsync(LabOrderDTO labOrderDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE LabOrders
SET 
    AppointmentId = @AppointmentId,
    PersonId = @PersonId,
    TestedByTechnicianId = @TestedByTechnicianId,
    BillId = @BillId,
    Date = @Date,
    Status = @status,
    Note = @Note
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", labOrderDTO.Id);
                    command.Parameters.AddWithValue("@AppointmentId", (object?)labOrderDTO.AppointmentId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PersonId", (object?)labOrderDTO.PersonId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TestedByTechnicianId", (object?)labOrderDTO.TestedByTechnicianId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BillId", labOrderDTO.BillId);
                    command.Parameters.AddWithValue("@Date", labOrderDTO.Date);
                    command.Parameters.AddWithValue("@status", (byte)labOrderDTO.Status);
                    command.Parameters.AddWithValue("@Note", (object?)labOrderDTO.Notes ?? DBNull.Value);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "LabOrder updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update labOrder.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public   async Task<Result<bool>> DeleteLabOrderAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM LabOrders WHERE Id = @id";
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
                            return new Result<bool>(true, "LabOrder deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete labOrder.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> ConfirmAsync(int billId,SqlConnection conn, SqlTransaction tran)
        {
            string query = @"UPDATE LabOrders
                                    SET 
                                        Status = @Status
                                    WHERE BillId = @BillId
                                    select @@ROWCOUNT";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@Status", (int)LabOrderStatus.Confirmed);
                command.Parameters.AddWithValue("@BillId", billId);

                object? result = await command.ExecuteScalarAsync();
                int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (rowAffected > 0)
                {
                    return new Result<bool>(true, "Appointment status changed.", true);
                }
                else
                {
                    return new Result<bool>(false, "Failed to change appointment status.", false);
                }

            }
        }
        public async Task<Result<bool>> ChangeStatusAsync(int id, LabOrderStatus status, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"UPDATE LabOrders
                                    SET 
                                        Status = @Status
                                    WHERE Id = @Id
                                    select @@ROWCOUNT";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@Status", (int)status);
                command.Parameters.AddWithValue("@Id", id);

                object? result = await command.ExecuteScalarAsync();
                int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (rowAffected > 0)
                {
                    return new Result<bool>(true, "Appointment status changed.", true);
                }
                else
                {
                    return new Result<bool>(false, "Failed to change appointment status.", false);
                }

            }
        }
        public async Task<Result<bool>> HasPenddingAsync(int? personId, int? appointmentId)
        {
            string query = "";
            SqlParameter parameter = null;
            if (personId.HasValue)
            {
                query = @"
select * from LabOrders 
where status = 1 and PersonId = @PersonId
";
                parameter = new SqlParameter("@PersonId", SqlDbType.Int) { Value = (int)personId };
            }
            else
            {
                query = @"
select * from LabOrders 
where status = 1 and AppointmentId = @AppointmentId
";
                parameter = new SqlParameter("@AppointmentId", SqlDbType.Int) { Value = appointmentId };
            }
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                   
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                    command.Parameters.Add(parameter);
                        bool isFound;
                        try
                        {
                            await connection.OpenAsync();
                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                isFound = reader.HasRows;
                            }
                            return new Result<bool>(true, "Check completed.", isFound);

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
