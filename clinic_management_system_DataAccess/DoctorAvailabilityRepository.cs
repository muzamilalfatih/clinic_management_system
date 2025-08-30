using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.DoctorAvailability;
using SharedClasses.DTOS.Doctors;
using SharedClasses.DTOS.LabOrderTests;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace clinic_management_system_DataAccess
{
    public class DoctorAvailabilityRepository
    {
        private readonly string _connectionString;
        public DoctorAvailabilityRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<List<AvailabilityInfoDTO>>> GetAllDoctorAvailabiltiesAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT DoctorAvailabilities.Id, DoctorAvailabilities.DayOfWeek, DoctorAvailabilities.StartTime, DoctorAvailabilities.EndTime
FROM     DoctorAvailabilities INNER JOIN
                  Doctors ON DoctorAvailabilities.DoctorId = Doctors.Id
WHERE  (Doctors.UserId = @UserID)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("UserId", userId);
                    List<AvailabilityInfoDTO> availabilitySlots = new List<AvailabilityInfoDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                 availabilitySlots.Add(new AvailabilityInfoDTO(
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                    (DayOfWeek) reader.GetByte(reader.GetOrdinal("DayOfWeek")),                          
                                     TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("StartTime"))),                          
                                     TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("EndTime")))
                                     ));
                            }

                            return new Result<List<AvailabilityInfoDTO>>(true, "Availabilties retrieves successfuly!", availabilitySlots);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<AvailabilityInfoDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> CreateAvailabiltiesAsync (List<CreateAvailabilityDTO> createAvailabilityDTO)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO DoctorAvailabilities (DoctorId, DayOfWeek, StartTime, EndTime) VALUES ");

            var parameters = new List<SqlParameter>();
            for (int i = 0; i < createAvailabilityDTO.Count; i++)
            {
                CreateAvailabilityDTO availability = createAvailabilityDTO[i];

                string valuesClause = $"(@DoctorId, @DayOfWeek{i}, @StartTime{i}, @EndTime{i})";

                if (i > 0)
                    queryBuilder.Append(", ");
                queryBuilder.Append(valuesClause);

                parameters.Add(new SqlParameter($"@DayOfWeek{i}", SqlDbType.Int) { Value = availability.DayOfWeek });
                parameters.Add(new SqlParameter($"@StartTime{i}", SqlDbType.Time) { Value = availability.StartTime.ToTimeSpan() });
                parameters.Add(new SqlParameter($"@EndTime{i}", SqlDbType.Time) { Value = availability.EndTime.ToTimeSpan() });
            }
            queryBuilder.AppendLine(";select @@ROWCOUNT");

            parameters.Add(new SqlParameter("@DoctorId", SqlDbType.Int) { Value = createAvailabilityDTO[0].DoctorId });

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        bool success = result != DBNull.Value ? Convert.ToInt32(result) > 0 : false;

                        return new Result<bool>(true, "Availability inserted successfully!", success);
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }
                }
            }
        }
        public async Task<Result<bool>> UpdateAvailabilityAsync(UpdateDoctorAvialbilityDTO updateDoctorAvialbilityDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE DoctorAvailabilities
SET DayOfWeek = @DayOfWeek,
    StartTime = @StartTime,
    EndTime = @EndTime

WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Id", updateDoctorAvialbilityDTO.Id);
                    command.Parameters.AddWithValue("@DayOfWeek", updateDoctorAvialbilityDTO.DayOfWeek);
                    command.Parameters.AddWithValue("@StartTime", updateDoctorAvialbilityDTO.StartTime.ToTimeSpan());
                    command.Parameters.AddWithValue("@EndTime", updateDoctorAvialbilityDTO.EndTime.ToTimeSpan());

                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Availability updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Availability not found.", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> DeleteAvailabilityAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM DoctorAvailabilities WHERE Id = @id;
select @@ROWCOUNT;";
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
                            return new Result<bool>(true, "Availability deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Availability not found.", false, 404);
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
