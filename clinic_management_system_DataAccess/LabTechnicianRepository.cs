using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Doctors;
using SharedClasses.DTOS.LabTechnician;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class LabTechnicianRepository
    {
        private readonly string _connectionString;

        public LabTechnicianRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<LabTechnicianDTO>> GetLabTechnicianInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM LabTechnicians WHERE Id = @id";
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
                                LabTechnicianDTO labTechnicianDTO = new LabTechnicianDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("UserId")),
                                     reader.GetInt32(reader.GetOrdinal("DeparmentId")),
                                     reader.GetByte(reader.GetOrdinal("PreviousExperienceYears")),
                                     reader.GetDateTime(reader.GetOrdinal("JoinDate"))
                                 );
                                return new Result<LabTechnicianDTO>(true, "LabTechnician found successfully", labTechnicianDTO);
                            }
                            else
                            {
                                return new Result<LabTechnicianDTO>(false, "LabTechnician not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabTechnicianDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> AddNewLabTechnicianAsync(CreateLabTechnicianDTO createLabTechnicianDTO, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO LabTechnicians
      (
      UserId
      ,DeparmentId
      ,PreviousExperienceYears
       ,JoinDate)
VALUES
      (
      @UserId
      ,@DeparmentId
      ,@PreviousExperienceYears
       ,@JoinDate);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@UserId", createLabTechnicianDTO.UserId);
                command.Parameters.AddWithValue("@DeparmentId", createLabTechnicianDTO.DepartmentId);
                command.Parameters.AddWithValue("@PreviousExperienceYears", createLabTechnicianDTO.PrevExperienceYears);
                command.Parameters.AddWithValue("@JoinDate", createLabTechnicianDTO.JoinDate);

                object? result = await command.ExecuteScalarAsync();
                int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (id > 0)
                {
                    return new Result<int>(true, "LabTechnician added successfully.", id);
                }
                else
                {
                    return new Result<int>(false, "Failed to add labTechnician.", -1);
                }

            }
        }

        public  async Task<Result<bool>> UpdateLabTechnicianAsync(UpdateLabTechnicianDTO updateLabTechnicianDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE LabTechnicians
SET 
    DeparmentId = @DeparmentId,
    PreviousExperienceYears = @PreviousExperienceYears,  
    JoinDate = @JoinDate

WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", updateLabTechnicianDTO.Id);
                    command.Parameters.AddWithValue("@DeparmentId", updateLabTechnicianDTO.DepartmentId);
                    command.Parameters.AddWithValue("@PreviousExperienceYears", updateLabTechnicianDTO.PrevExperienceYears);
                    command.Parameters.AddWithValue("@JoinDate", updateLabTechnicianDTO.JoinDate);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "LabTechnician updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "LabTechnician not found.", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<LabTechnicianProfileDTO>> GetProfileAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT LabDepartments.Name as Department, LabTechnicians.PreviousExperienceYears, LabTechnicians.JoinDate
FROM     LabTechnicians INNER JOIN
                  LabDepartments ON LabTechnicians.DeparmentId = LabDepartments.Id

WHERE LabTechnicians.UserId = @UserId";
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
                                LabTechnicianProfileDTO labTechnicianProfile = new LabTechnicianProfileDTO
                                 (
                                     reader.GetString(reader.GetOrdinal("Department")),
                                     reader.GetByte(reader.GetOrdinal("PreviousExperienceYears")),
                                     reader.GetDateTime(reader.GetOrdinal("JoinDate"))
                                 );
                                return new Result<LabTechnicianProfileDTO>(true, "Doctor found successfully", labTechnicianProfile);
                            }
                            else
                            {
                                return new Result<LabTechnicianProfileDTO>(false, "Doctor not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabTechnicianProfileDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }

        }
        public  async Task<Result<bool>> DeleteLabTechnicianAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM LabTechnicians WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync() ;
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "LabTechnician deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete labTechnician.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<int>> GetIdAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select Id from LabTechnicians
where UserId = @UserId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "LabTechnician id retrieved successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Id not found.", -1, 404);
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
