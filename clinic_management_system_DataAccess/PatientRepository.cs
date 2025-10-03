using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Patients;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class PatientRepository
    {
        private readonly string _connectionString;

        public PatientRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<PatientDTO>> GetPatientInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Patients WHERE Id = @id";
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
                                PatientDTO patientDTO = new PatientDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("UserId")),
                                     reader.GetString(reader.GetOrdinal("MedicalHistroy"))
                                 );
                                return new Result<PatientDTO>(true, "Patient found successfully", patientDTO);
                            }
                            else
                            {
                                return new Result<PatientDTO>(false, "Patient not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PatientDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<PatientProfileDTO>> GetProfileAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Patients.MedicalHistroy, Patients.Allergies
FROM     Patients INNER JOIN
                  Users ON Patients.UserId = Users.Id
where Users.Id = @UserId";
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
                                PatientProfileDTO patientProfileDTO = new PatientProfileDTO
                                 (
                                     reader.GetString(reader.GetOrdinal("MedicalHistroy")),
                                     reader.GetString(reader.GetOrdinal("Allergies"))
                                 );
                                return new Result<PatientProfileDTO>(true, "Patient found successfully", patientProfileDTO);
                            }
                            else
                            {
                                return new Result<PatientProfileDTO>(false, "Patient not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PatientProfileDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }

        }
        public  async Task<Result<int>> AddNewPatientAsync(CreatePatientDTO createPatientDTO, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO Patients
      (
      UserId
      ,MedicalHistroy
       ,Allergies)
VALUES
      (
      @UserId
      ,@MedicalHistroy,
       @Allergies);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn,tran))
            {
                command.Parameters.AddWithValue("@UserId", createPatientDTO.userId);
                command.Parameters.AddWithValue("@MedicalHistroy", createPatientDTO.medicalHistory);
                command.Parameters.AddWithValue("@Allergies", createPatientDTO.allergies);
                try
                {
                    object result = await command.ExecuteScalarAsync();
                    int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    if (id > 0)
                    {
                        return new Result<int>(true, "Patient added successfully.", id);
                    }
                    else
                    {
                        return new Result<int>(false, "Failed to add patient.", -1);
                    }
                }
                catch (Exception ex)
                {
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }

            }
        }
        public async Task<Result<int>> AddNewPatientAsync(int userId, CreatePatientDTO createPatientDTO, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO Patients
      (
      UserId
      ,MedicalHistroy
       ,Allergies)
VALUES
      (
      @UserId
      ,@MedicalHistroy,
       @Allergies);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@MedicalHistroy", createPatientDTO.medicalHistory);
                command.Parameters.AddWithValue("@Allergies", createPatientDTO.allergies);
                try
                {
                    object result = await command.ExecuteScalarAsync();
                    int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    if (id > 0)
                    {
                        return new Result<int>(true, "Patient added successfully.", id);
                    }
                    else
                    {
                        return new Result<int>(false, "Failed to add patient.", -1);
                    }
                }
                catch (Exception ex)
                {
                    return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                }

            }
        }

        public async Task<Result<bool>> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Patients
SET 
    MedicalHistroy = @MedicalHistroy,
    Allergies = @Allergies
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", updatePatientDTO.Id);
                    command.Parameters.AddWithValue("@MedicalHistroy", updatePatientDTO.medicalHistory);
                    command.Parameters.AddWithValue("@Allergies", updatePatientDTO.allergies);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Patient updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Patient not found.", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeletePatientAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Patients WHERE Id = @id";
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
                            return new Result<bool>(true, "Patient deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete patient.", false);
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
                string query = @"
select Id from Patients
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
