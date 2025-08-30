using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class PrescriptionRepository
    {
        private readonly string _connectionString;

        public PrescriptionRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<PrescriptionDTO>> GetPrescriptionInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Prescriptions WHERE Id = @id";
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
                                PrescriptionDTO prescriptionDTO = new PrescriptionDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("AppoinmentId")),
                                     reader.GetDateTime(reader.GetOrdinal("Date")),
                                     reader.GetString(reader.GetOrdinal("Notes"))
                                 );
                                return new Result<PrescriptionDTO>(true, "Prescription found successfully", prescriptionDTO);
                            }
                            else
                            {
                                return new Result<PrescriptionDTO>(false, "Prescription not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PrescriptionDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> AddNewPrescriptionAsync(PrescriptionDTO prescriptionDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO Prescriptions
      (
      AppoinmentId
      ,Date
      ,Notes)
VALUES
      (
      @AppoinmentId
      ,@Date
      ,@Notes);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AppoinmentId", prescriptionDTO.appoinmentId);
                    command.Parameters.AddWithValue("@Date", prescriptionDTO.date);
                    command.Parameters.AddWithValue("@Notes", prescriptionDTO.notes);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Prescription added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add prescription.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> UpdatePrescriptionAsync(PrescriptionDTO prescriptionDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Prescriptions
SET 
    AppoinmentId = @AppoinmentId,
    Date = @Date,
    Notes = @Notes
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", prescriptionDTO.id);
                    command.Parameters.AddWithValue("@AppoinmentId", prescriptionDTO.appoinmentId);
                    command.Parameters.AddWithValue("@Date", prescriptionDTO.date);
                    command.Parameters.AddWithValue("@Notes", prescriptionDTO.notes);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "Prescription updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update prescription.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeletePrescriptionAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Prescriptions WHERE Id = @id";
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
                            return new Result<bool>(true, "Prescription deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete prescription.", false);
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
