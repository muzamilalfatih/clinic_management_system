using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class PrescriptionItemRepository
    {
        private readonly string _connectionString;

        public PrescriptionItemRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<PrescriptionItemDTO>> GetPrescriptionItemInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM PrescriptionItems WHERE Id = @id";
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
                                PrescriptionItemDTO prescriptionItemDTO = new PrescriptionItemDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("PrescriptionId")),
                                     reader.GetString(reader.GetOrdinal("MedicineName")),
                                     reader.GetString(reader.GetOrdinal("Dosage")),
                                     reader.GetString(reader.GetOrdinal("Frequency")),
                                     reader.GetString(reader.GetOrdinal("Duration"))
                                 );
                                return new Result<PrescriptionItemDTO>(true, "PrescriptionItem found successfully", prescriptionItemDTO);
                            }
                            else
                            {
                                return new Result<PrescriptionItemDTO>(false, "PrescriptionItem not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PrescriptionItemDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> AddNewPrescriptionItemAsync(PrescriptionItemDTO prescriptionItemDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO PrescriptionItems
      (
      PrescriptionId
      ,MedicineName
      ,Dosage
      ,Frequency
      ,Duration)
VALUES
      (
      @PrescriptionId
      ,@MedicineName
      ,@Dosage
      ,@Frequency
      ,@Duration);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PrescriptionId", prescriptionItemDTO.prescriptionId);
                    command.Parameters.AddWithValue("@MedicineName", prescriptionItemDTO.medicineName);
                    command.Parameters.AddWithValue("@Dosage", prescriptionItemDTO.dosage);
                    command.Parameters.AddWithValue("@Frequency", prescriptionItemDTO.frequency);
                    command.Parameters.AddWithValue("@Duration", prescriptionItemDTO.duration);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "PrescriptionItem added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add prescriptionItem.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> UpdatePrescriptionItemAsync(PrescriptionItemDTO prescriptionItemDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE PrescriptionItems
SET 
    PrescriptionId = @PrescriptionId,
    MedicineName = @MedicineName,
    Dosage = @Dosage,
    Frequency = @Frequency,
    Duration = @Duration
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", prescriptionItemDTO.id);
                    command.Parameters.AddWithValue("@PrescriptionId", prescriptionItemDTO.prescriptionId);
                    command.Parameters.AddWithValue("@MedicineName", prescriptionItemDTO.medicineName);
                    command.Parameters.AddWithValue("@Dosage", prescriptionItemDTO.dosage);
                    command.Parameters.AddWithValue("@Frequency", prescriptionItemDTO.frequency);
                    command.Parameters.AddWithValue("@Duration", prescriptionItemDTO.duration);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "PrescriptionItem updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update prescriptionItem.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeletePrescriptionItemAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM PrescriptionItems WHERE Id = @id";
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
                            return new Result<bool>(true, "PrescriptionItem deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete prescriptionItem.", false);
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
