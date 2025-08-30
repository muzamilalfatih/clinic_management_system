using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class AppointmentRepository
     {
        private readonly string _connectionString;

        public AppointmentRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<AppointmentDTO>> GetAppointmentInfoByIDAsync(int id)
        {
         using (SqlConnection connection = new SqlConnection(_connectionString))
         {
            string query = @"SELECT * FROM Appointments WHERE Id = @id";
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
                                AppointmentDTO appointmentDTO =  new AppointmentDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("PatientId")),
                                     reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                     reader.GetInt32(reader.GetOrdinal("BillId")),
                                     reader.GetDateTime(reader.GetOrdinal("AppointmentTime")),
                                     reader.GetByte(reader.GetOrdinal("Status")),
                                     reader.GetString(reader.GetOrdinal("notes")),
                                     reader.GetInt32(reader.GetOrdinal("ParentAppoinmentId"))
                                 );
                                return new Result<AppointmentDTO>(true, "Appointment found successfully", appointmentDTO);
                             }
                             else
                             {
                                return new Result<AppointmentDTO>(false, "Appointment not found.", null, 404);
                             }
                         }
                     }
                     catch (Exception ex)
                     {
                         return new Result<AppointmentDTO>(false, "An unexpected error occurred on the server.", null, 500);
                     }

                 }
            }
         }

        public  async Task<Result<int>> AddNewAppointmentAsync(AppointmentDTO appointmentDTO)
         {
         using (SqlConnection connection = new SqlConnection(_connectionString))
         {
            string query = @"
INSERT INTO Appointments
      (
      PatientId
      ,DoctorId
      ,BillId
      ,AppointmentTime
      ,Status
      ,notes
      ,ParentAppoinmentId)
VALUES
      (
      @PatientId
      ,@DoctorId
      ,@BillId
      ,@AppointmentTime
      ,@Status
      ,@notes
      ,@ParentAppoinmentId);
SELECT SCOPE_IDENTITY();
";
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                    command.Parameters.AddWithValue("@PatientId", appointmentDTO.patientId);
                    command.Parameters.AddWithValue("@DoctorId", appointmentDTO.doctorId);
                    command.Parameters.AddWithValue("@BillId", appointmentDTO.billId);
                    command.Parameters.AddWithValue("@AppointmentTime", appointmentDTO.appointmentTime);
                    command.Parameters.AddWithValue("@Status", appointmentDTO.status);
                    command.Parameters.AddWithValue("@notes", appointmentDTO.notes);
                    command.Parameters.AddWithValue("@ParentAppoinmentId", appointmentDTO.parentAppoinmentId);


                     try
                     {
                         await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                         if (id > 0)
                         {
                             return new Result<int>(true, "Appointment added successfully.", id);
                          }
                          else
                          {
                             return new Result<int>(false, "Failed to add appointment.", -1);
                          }
                     }
                     catch (Exception ex)
                     {
                         return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                     }

                 }
             }
         }

        public  async Task<Result<int>> UpdateAppointmentAsync(AppointmentDTO appointmentDTO)
        {
         using (SqlConnection connection = new SqlConnection(_connectionString))
         {
            string query = @"
UPDATE Appointments
SET 
    PatientId = @PatientId,
    DoctorId = @DoctorId,
    BillId = @BillId,
    AppointmentTime = @AppointmentTime,
    Status = @Status,
    notes = @notes,
    ParentAppoinmentId = @ParentAppoinmentId
WHERE Id = @Id;
select @@ROWCOUNT";
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                    command.Parameters.AddWithValue("@Id", appointmentDTO.id);
                    command.Parameters.AddWithValue("@PatientId", appointmentDTO.patientId);
                    command.Parameters.AddWithValue("@DoctorId", appointmentDTO.doctorId);
                    command.Parameters.AddWithValue("@BillId", appointmentDTO.billId);
                    command.Parameters.AddWithValue("@AppointmentTime", appointmentDTO.appointmentTime);
                    command.Parameters.AddWithValue("@Status", appointmentDTO.status);
                    command.Parameters.AddWithValue("@notes", appointmentDTO.notes);
                    command.Parameters.AddWithValue("@ParentAppoinmentId", appointmentDTO.parentAppoinmentId);


                     try
                     {
                         await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                         if (rowAffected > 0)
                         {
                             return new Result<int>(true, "Appointment updated successfully.", rowAffected);
                          }
                          else
                          {
                             return new Result<int>(false, "Failed to update appointment.", -1);
                          }
                     }
                     catch (Exception ex)
                     {
                         return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                     }

                 }
            }
         }

        public  async Task<Result<bool>> DeleteAppointmentAsync(int id)
        {
         using (SqlConnection connection = new SqlConnection(_connectionString))
         {
            string query = @"DELETE FROM Appointments WHERE Id = @id";
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
                             return new Result<bool>(true, "Appointment deleted successfully.", true);
                          }
                          else
                          {
                             return new Result<bool>(false, "Failed to delete appointment.", false);
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
