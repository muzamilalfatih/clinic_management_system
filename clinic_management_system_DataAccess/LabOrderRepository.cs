using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
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
                                     reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                                     reader.GetInt32(reader.GetOrdinal("PatientId")),
                                     reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                     reader.GetInt32(reader.GetOrdinal("BillId")),
                                     reader.GetDateTime(reader.GetOrdinal("Date")),
                                     reader.GetByte(reader.GetOrdinal("status")),
                                     reader.GetString(reader.GetOrdinal("Note"))
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

        public  async Task<Result<int>> AddNewLabOrderAsync(LabOrderDTO labOrderDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO LabOrders
      (
      AppointmentId
      ,PatientId
      ,DoctorId
      ,BillId
      ,Date
      ,status
      ,Note)
VALUES
      (
      @AppointmentId
      ,@PatientId
      ,@DoctorId
      ,@BillId
      ,@Date
      ,@status
      ,@Note);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AppointmentId", labOrderDTO.appointmentId);
                    command.Parameters.AddWithValue("@PatientId", labOrderDTO.patientId);
                    command.Parameters.AddWithValue("@DoctorId", labOrderDTO.doctorId);
                    command.Parameters.AddWithValue("@BillId", labOrderDTO.billId);
                    command.Parameters.AddWithValue("@Date", labOrderDTO.date);
                    command.Parameters.AddWithValue("@status", labOrderDTO.status);
                    command.Parameters.AddWithValue("@Note", labOrderDTO.note);


                    try
                    {
                        await connection.OpenAsync();
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
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

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
    PatientId = @PatientId,
    DoctorId = @DoctorId,
    BillId = @BillId,
    Date = @Date,
    status = @status,
    Note = @Note
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", labOrderDTO.id);
                    command.Parameters.AddWithValue("@AppointmentId", labOrderDTO.appointmentId);
                    command.Parameters.AddWithValue("@PatientId", labOrderDTO.patientId);
                    command.Parameters.AddWithValue("@DoctorId", labOrderDTO.doctorId);
                    command.Parameters.AddWithValue("@BillId", labOrderDTO.billId);
                    command.Parameters.AddWithValue("@Date", labOrderDTO.date);
                    command.Parameters.AddWithValue("@status", labOrderDTO.status);
                    command.Parameters.AddWithValue("@Note", labOrderDTO.note);


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


    }
}
