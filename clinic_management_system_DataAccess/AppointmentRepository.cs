using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Appointment;
using SharedClasses.DTOS.Doctors;
using SharedClasses.Enums;
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
        public async Task<Result<AppointmentDTO>> GetAppointmentInfoById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select * from Appointments";

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
                                AppointmentDTO appointmentDTO = new AppointmentDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("PatientId")),
                                     reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                     reader.GetDecimal(reader.GetOrdinal("Fee")),
                                     reader.GetInt32(reader.GetOrdinal("BillId")),
                                     reader.GetDateTime(reader.GetOrdinal("Date")),
                                    (AppointmentStatus)reader.GetByte(reader.GetOrdinal("Status")),
                                     reader.IsDBNull(reader.GetOrdinal("Notes"))
                                        ? (string?)null
                                        : reader.GetString(reader.GetOrdinal("Notes")),
                                     reader.IsDBNull(reader.GetOrdinal("Symptoms"))
                                        ? (string?)null
                                        : reader.GetString(reader.GetOrdinal("Symptoms")),
                                     reader.IsDBNull(reader.GetOrdinal("Diagnoses"))
                                        ? (string?)null
                                        : reader.GetString(reader.GetOrdinal("Diagnoses")),
                                     reader.IsDBNull(reader.GetOrdinal("ParentAppoinmentId"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("ParentAppoinmentId"))
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
        public async Task<Result<AppointmentInfoDTO>> GetFullAppointmentInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"

   SELECT 
    a.Id,
    CONCAT(pp.FirstName, ' ', pp.SecondName, ' ', pp.ThirdName, ' ', pp.LastName) AS Patient,
    CONCAT(pd.FirstName, ' ', pd.SecondName, ' ', pd.ThirdName, ' ', pd.LastName) AS Doctor,
    a.Fee,
    a.Symptoms,
    a.Diagnoses,
    a.Date,
    a.Status,
    a.Notes,
    a.ParentAppoinmentId
FROM Appointments a
JOIN Patients p ON a.PatientId = p.Id
JOIN Users uPatient ON p.UserId = uPatient.Id
JOIN People pp ON uPatient.PersonId = pp.Id
JOIN Doctors d ON a.DoctorId = d.Id
JOIN Users uDoctor ON d.UserId = uDoctor.Id
JOIN People pd ON uDoctor.PersonId = pd.Id
WHERE a.Id = @id;

    ";
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
                                AppointmentInfoDTO appointmentDTO = new AppointmentInfoDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetString(reader.GetOrdinal("Patient")),
                                     reader.GetString(reader.GetOrdinal("Doctor")),
                                     reader.GetDecimal(reader.GetOrdinal("Fee")),
                                     reader.GetDateTime(reader.GetOrdinal("Date")),
                                    (AppointmentStatus)reader.GetByte(reader.GetOrdinal("Status")),
                                     reader.IsDBNull(reader.GetOrdinal("Notes"))
                                        ? (string?)null
                                        : reader.GetString(reader.GetOrdinal("Notes")),
                                     reader.IsDBNull(reader.GetOrdinal("Symptoms"))
                                        ? (string?)null
                                        : reader.GetString(reader.GetOrdinal("Symptoms")),
                                     reader.IsDBNull(reader.GetOrdinal("Diagnoses"))
                                        ? (string?)null
                                        : reader.GetString(reader.GetOrdinal("Diagnoses")),
                                     reader.IsDBNull(reader.GetOrdinal("ParentAppoinmentId"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("ParentAppoinmentId"))
                                 );
                                return new Result<AppointmentInfoDTO>(true, "Appointment found successfully", appointmentDTO);
                            }
                            else
                            {
                                return new Result<AppointmentInfoDTO>(false, "Appointment not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<AppointmentInfoDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<int>> AddNewAppointmentAsync(AddNewAppointmentDTO addNewAppointmentDTO, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO Appointments
      (
      PatientId
      ,DoctorId
      ,Fee 
      ,BillId
      ,Date
      ,Notes
      ,ParentAppoinmentId)
VALUES
      (
      @PatientId
      ,@DoctorId
      ,@Fee
      ,@BillId
      ,@Date
      ,@Notes
      ,@ParentAppoinmentId);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@PatientId", addNewAppointmentDTO.PatientId);
                command.Parameters.AddWithValue("@DoctorId", addNewAppointmentDTO.DoctorId);
                command.Parameters.AddWithValue("@Fee", addNewAppointmentDTO.Fee);
                command.Parameters.AddWithValue("@BillId", addNewAppointmentDTO.BillId);
                command.Parameters.AddWithValue("@Date", addNewAppointmentDTO.Date);
                command.Parameters.AddWithValue("@Notes", (object?)addNewAppointmentDTO.Notes ?? DBNull.Value);
                command.Parameters.AddWithValue("@ParentAppoinmentId", (object?)addNewAppointmentDTO.ParentAppointmentId ?? DBNull.Value);

                object? result = await command.ExecuteScalarAsync();
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
        }

        public async Task<Result<bool>> UpdateAppointmentAsync(UpdateAppointmentDTO updateAppointmentDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Appointments
SET 
    DoctorId = @DoctorId,
    Fee = @Fee, 
    Date = @Date,
    Notes = @Notes,
    ParentAppointmentId = @ParentAppointmentId
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", updateAppointmentDTO.Id);
                    command.Parameters.AddWithValue("@DoctorId", updateAppointmentDTO.DoctorId);
                    command.Parameters.AddWithValue("@Fee", updateAppointmentDTO.Fee);
                    command.Parameters.AddWithValue("@Date", updateAppointmentDTO.Date);
                    command.Parameters.AddWithValue("@Notes", updateAppointmentDTO.Notes);
                    command.Parameters.AddWithValue("@ParentAppointmentId", updateAppointmentDTO.ParentAppointmentId);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Appointment updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to update appointment.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteAppointmentAsync(int id)
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
                        object? result = await command.ExecuteScalarAsync();
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

        public async Task<Result<bool>> HasPenddingAppointment(int pateintId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select * from Appointments
                                where PatientId = @pateintId and status = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@pateintId", pateintId);
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
        public async Task<Result<bool>> Cancel(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Appointments
                                    SET 
                                        Status = 5
                                    WHERE Id = @Id and Status not in (5 , 3)
                                    select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", (int)AppointmentStatus.Cancelled);
                    command.Parameters.AddWithValue("@Id", id);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Appointment Cancelled.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Not allowed to canecll.", false, 400);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> Reschedule(RescheduleDTO rescheduleDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Appointments
                                    SET 
                                        Date = @date
                                    WHERE Id = @Id
                                    select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@date", rescheduleDTO.NewDate);
                    command.Parameters.AddWithValue("@Id", rescheduleDTO.Id);

                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Appointment reschedule.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to reschedule.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }

        }
        public async Task<Result<List<AppointmentInfoDTO>>> GetAllAppointment(AppointmentFilterDTO filter)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT 
        a.Id,
        CONCAT(pp.FirstName, ' ', pp.SecondName, ' ', pp.ThirdName, ' ', pp.LastName) AS Patient,
        CONCAT(pd.FirstName, ' ', pd.SecondName, ' ', pd.ThirdName, ' ', pd.LastName) AS Doctor,
        a.Fee,
        a.Date,
	    a.Status,
	    a.notes,
	    a.ParentAppoinmentId
    FROM Appointments a
    INNER JOIN Users up ON a.PatientId = up.Id
    INNER JOIN People pp ON up.PersonId = pp.Id

    INNER JOIN Users ud ON a.DoctorId = ud.Id
    INNER JOIN People pd ON ud.PersonId = pd.Id

    WHERE (@DoctorId IS NULL OR a.DoctorId = @DoctorId)
  AND (@PatientId IS NULL OR a.PatientId = @PatientId)
  AND (@StartDate IS NULL OR a.Date >= @StartDate)
  AND (@EndDate IS NULL OR a.Date <= @EndDate)
ORDER BY a.Date;
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int offset = (filter.PageNumber - 1) * filter.PageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", filter.PageSize);

                    command.Parameters.AddWithValue("@DoctorId", filter.DoctorId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PatientId", filter.PatientId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", filter.StartDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", filter.EndDate ?? (object)DBNull.Value);


                    List<AppointmentInfoDTO> appointments = new List<AppointmentInfoDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            while (reader.Read())
                            {
                                appointments.Add(new AppointmentInfoDTO(
                                        reader.GetInt32(reader.GetOrdinal("Id")),
                                        reader.GetString(reader.GetOrdinal("Patient")),
                                        reader.GetString(reader.GetOrdinal("Doctor")),
                                        reader.GetDecimal(reader.GetOrdinal("Fee")),
                                        reader.GetDateTime(reader.GetOrdinal("Date")),
                                        (AppointmentStatus)reader.GetByte(reader.GetOrdinal("Status")),
                                        reader.GetString(reader.GetOrdinal("Notes")),
                                        reader.GetString(reader.GetOrdinal("Symptoms")),
                                        reader.GetString(reader.GetOrdinal("Diagnoses")),
                                        reader.GetInt32(reader.GetOrdinal("ParentAppoinmentId"))
                                    ));
                            }

                            return new Result<List<AppointmentInfoDTO>>(true, "Appointments retrieved successfully", appointments);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<AppointmentInfoDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> ChangeStatus(int billId, AppointmentStatus status, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"UPDATE Appointments
                                    SET 
                                        Status = @Status
                                    WHERE BillId = @BillId
                                    select @@ROWCOUNT";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@Status", (int)status);
                command.Parameters.AddWithValue("@BillId", billId);

                object result = await command.ExecuteScalarAsync();
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
        public async Task<Result<bool>> IsValidAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select Id from Appointments
                                where Id = @Id and status = 2";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
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
        public async Task<Result<bool>> IsExistAsync(int billId, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"select Id from Appointments
                                where BillId = @BillId";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@BillId", billId);
                bool isFound;
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    isFound = reader.HasRows;
                }
                return new Result<bool>(true, "Check completed.", isFound);

            }
        }
    }
}
