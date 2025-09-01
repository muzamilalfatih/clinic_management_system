using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Doctors;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class DoctorRepository
    {
        private readonly string _connectionString;
        public DoctorRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<DoctorDTO>> GetDoctorInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Doctors WHERE Id = @id";
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
                                DoctorDTO doctorDTO = new DoctorDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("UserId")),
                                     reader.GetInt32(reader.GetOrdinal("SpecializationId")),
                                     reader.GetByte(reader.GetOrdinal("PreviousExperienceYears")),
                                     reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                                     reader.GetString(reader.GetOrdinal("Bio")),
                                     reader.GetDecimal(reader.GetOrdinal("ConsultationFee"))
                                 );
                                return new Result<DoctorDTO>(true, "Doctor found successfully", doctorDTO);
                            }
                            else
                            {
                                return new Result<DoctorDTO>(false, "Doctor not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<DoctorDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<UpdateDoctorDTO>> GetOldDataAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Id, SpecializationId, PreviousExperienceYears, JoinDate, Bio, ConsultationFee
FROM     Doctors
WHERE  (Id = @id)";
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
                                UpdateDoctorDTO doctorDTO = new UpdateDoctorDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("SpecializationId")),
                                     reader.GetByte(reader.GetOrdinal("PreviousExperienceYears")),
                                     reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                                     reader.GetString(reader.GetOrdinal("Bio")),
                                     reader.GetDecimal(reader.GetOrdinal("ConsulationFee"))
                                 );
                                return new Result<UpdateDoctorDTO>(true, "Doctor found successfully", doctorDTO);
                            }
                            else
                            {
                                return new Result<UpdateDoctorDTO>(false, "Doctor not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<UpdateDoctorDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<int>> AddNewDoctorAsync(CreateDoctorDTO createDoctorDTO, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO Doctors
      (
      UserId
      ,SpecializationId
      ,PreviousExperienceYears
       ,JoinDate
      ,Bio
       ,ConsultationFee )
VALUES
      (
      @UserId
      ,@SpecializationId
      ,@PreviousExperienceYears
       ,@JoinDate
      ,@Bio
       ,@consultationFee);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@UserId", createDoctorDTO.userId);
                command.Parameters.AddWithValue("@SpecializationId", createDoctorDTO.specializationId);
                command.Parameters.AddWithValue("@PreviousExperienceYears", createDoctorDTO.prevExperienceYears);
                command.Parameters.AddWithValue("@JoinDate", createDoctorDTO.joinDate);
                command.Parameters.AddWithValue("@Bio", createDoctorDTO.bio);
                command.Parameters.AddWithValue("@consultationFee", createDoctorDTO.consultationFee);


                object? result = await command.ExecuteScalarAsync();
                int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (id > 0)
                {
                    return new Result<int>(true, "Doctor added successfully.", id);
                }

                return new Result<int>(false, "Failed to add doctor.", -1, 500);
            }
        }
        public async Task<Result<bool>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Doctors
SET 
    SpecializationId = @SpecializationId,
    PreviousExperienceYears = @PreviousExperienceYears,  
    JoinDate = @JoinDate,
    Bio = @Bio,
    ConsultationFee = @ConsultationFees

WHERE UserId = @UserId;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", updateDoctorDTO.userId);
                    command.Parameters.AddWithValue("@SpecializationId", updateDoctorDTO.specializationId);
                    command.Parameters.AddWithValue("@PreviousExperienceYears", updateDoctorDTO.prevExperienceYears);
                    command.Parameters.AddWithValue("@JoinDate", updateDoctorDTO.joinDate);
                    command.Parameters.AddWithValue("@ConsultationFee", updateDoctorDTO.consultationFee);
                    command.Parameters.AddWithValue("@Bio", updateDoctorDTO.bio);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Doctor updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Doctor not found.", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<DoctorProfileDTO>> GetProfileAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Specializations.Name as Specialization, Doctors.PreviousExperienceYears, Doctors.JoinDate, Doctors.Bio, Doctors.ConsultationFee
FROM     Users INNER JOIN
                  Doctors ON Users.Id = Doctors.UserId INNER JOIN
                  Specializations ON Doctors.SpecializationId = Specializations.Id
WHERE  (Users.Id = @UserId);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                DoctorProfileDTO doctorProfileDTO = new DoctorProfileDTO
                                 (
                                     reader.GetString(reader.GetOrdinal("Specialization")),
                                     reader.GetByte(reader.GetOrdinal("PreviousExperienceYears")),
                                     reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                                     reader.GetString(reader.GetOrdinal("Bio")),
                                     reader.GetDecimal(reader.GetOrdinal("ConsultationFee"))
                                 );
                                return new Result<DoctorProfileDTO>(true, "Doctor found successfully", doctorProfileDTO);
                            }
                            else
                            {
                                return new Result<DoctorProfileDTO>(false, "Doctor not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<DoctorProfileDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }

        }
        public async Task<Result<bool>> DeleteDoctorAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Doctors WHERE Id = @id";
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
                            return new Result<bool>(true, "Doctor deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete doctor.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<List<DoctorInfoDTO>>> GetDoctorsAsync(FilterDoctorDTO filter)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Doctors.Id, ( People.FirstName + ' '+ People.SecondName + ' '+ People.ThirdName + ' '+ People.LastName) as Name ,Specializations.Name as Specialization, Doctors.PreviousExperienceYears , Doctors.JoinDate ,Doctors.Bio, Doctors.ConsultationFee, DoctorAvailabilities.DayOfWeek, 
                  DoctorAvailabilities.StartTime, DoctorAvailabilities.EndTime
FROM     Users INNER JOIN
                  People ON Users.PersonId = People.Id INNER JOIN
                  Doctors ON Users.Id = Doctors.UserId INNER JOIN
                  Specializations ON Doctors.SpecializationId = Specializations.Id LEFT OUTER JOIN
                  DoctorAvailabilities ON Doctors.Id = DoctorAvailabilities.DoctorId
WHERE (@specializationId is Null or  Specializations.Id = @specializationId)
  AND (
      @dayOfWeek IS NULL OR DoctorAvailabilities.DayOfWeek = @dayOfWeek
  )
  AND (
      @time IS NULL OR (
          DoctorAvailabilities.StartTime <= @time AND DoctorAvailabilities.EndTime >= @time
      )
  )
ORDER BY Doctors.Id, DoctorAvailabilities.DayOfWeek
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int offset = (filter.PageNumber - 1) * filter.PageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", filter.PageSize);

                    command.Parameters.AddWithValue("@specializationId", filter.SpecializationId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@dayOfWeek", filter.DayOfWeek ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@time", filter.Time ?? (object)DBNull.Value);


                    List<DoctorInfoDTO> doctors = new List<DoctorInfoDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            
                            var doctorLookup = new Dictionary<int, DoctorInfoDTO>();

                            while (reader.Read())
                            {
                                int doctorId = reader.GetInt32(reader.GetOrdinal("Id"));

                                if (!doctorLookup.TryGetValue(doctorId, out var doctor))
                                {
                                    doctor = new DoctorInfoDTO(
                                        doctorId,
                                        reader.GetString(reader.GetOrdinal("Name")),
                                        reader.GetString(reader.GetOrdinal("Specialization")),
                                        reader.GetByte(reader.GetOrdinal("PreviousExperienceYears")),
                                        reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                                        reader.GetString(reader.GetOrdinal("Bio")),
                                        reader.GetDecimal(reader.GetOrdinal("ConsultationFee")),
                                        new List<AvailabilitySlotDTO>()
                                        );

                                    doctorLookup[doctorId] = doctor;
                                    doctors.Add(doctor);
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("DayOfWeek"))
                                    || !reader.IsDBNull(reader.GetOrdinal("StartTime"))
                                    || !reader.IsDBNull(reader.GetOrdinal("EndTime")))
                                {
                                    doctor.Availabilities.Add(new AvailabilitySlotDTO
                                    (
                                        (DayOfWeek)reader.GetByte(reader.GetOrdinal("DayOfWeek")),
                                        TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("StartTime"))),
                                        TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("EndTime")))
                                    ));
                                }
                            }

                            return new Result<List<DoctorInfoDTO>>(true, "Doctors retrieved successfully", doctors);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<DoctorInfoDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<int>> GetDoctorIdAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Id FROM Doctors 
WHERE UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Doctor id retrieved successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "User not found.", -1, 404);
                        }

                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }
    }
}
