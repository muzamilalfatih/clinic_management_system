using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.LabOrderResults;
using SharedClasses.DTOS.PescriptionItem;
using System.Data;
using System.Text;
namespace clinic_management_system_DataAccess
{
    public class PrescriptionItemRepository
    {
        private readonly string _connectionString;

        public PrescriptionItemRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        private (string query, List<SqlParameter> parameters) _queryBuilder(List<AddNewPrescriptionItemDTO> items, int prescriptionId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO PrescriptionItems (PrescriptionId, MedicineName, Dosage, Frequency, Duration) VALUES ");

            var parameters = new List<SqlParameter>();
            for (int i = 0; i < items.Count; i++)
            {
                AddNewPrescriptionItemDTO item
                    = items[i];

                string valuesClause = $"(@PrescriptionId, @MedicineName{i}, @Dosage{i}, @Frequency{i}, @Duration{i})";

                if (i > 0)
                    queryBuilder.Append(", ");
                queryBuilder.Append(valuesClause);

                parameters.Add(new SqlParameter($"@MedicineName{i}", SqlDbType.NVarChar, 100) { Value = item.MedicineName });
                parameters.Add(new SqlParameter($"@Dosage{i}", SqlDbType.NVarChar, 100) { Value = item.Dosage });
                parameters.Add(new SqlParameter($"@Frequency{i}", SqlDbType.NVarChar, 100) { Value = item.Frequency });
                parameters.Add(new SqlParameter($"@Duration{i}", SqlDbType.NVarChar, 100) { Value = item.Duration });
            }
            queryBuilder.AppendLine(";SELECT SCOPE_IDENTITY()");
            // One shared LabOrderId param
            parameters.Add(new SqlParameter("@PrescriptionId", SqlDbType.Int) { Value = prescriptionId });

            return (queryBuilder.ToString(), parameters);
        }
        public async Task<Result<bool>> AddNewAsync(List<AddNewPrescriptionItemDTO> items, int prescriptionId, SqlConnection conn, SqlTransaction tran)
        {

            if (items == null || items.Count == 0)
                return new Result<bool>(false, "No data provided!", false, 400);

            (string query, List<SqlParameter> parameters) = _queryBuilder(items, prescriptionId);


            using (SqlCommand command = new SqlCommand(query.ToString(), conn, tran))
            {
                command.Parameters.AddRange(parameters.ToArray());

                object? result = await command.ExecuteScalarAsync();
                bool success = result != DBNull.Value ? Convert.ToInt32(result) > 0 : false;

                return new Result<bool>(true, "Items inserted successfully!", success);
            }

        }
        public async Task<Result<PrescriptionItemDTO>> GetInfoByIDAsync(int id)
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
        public async Task<Result<List<PrescriptionItemDTO>>> GetAllAsync(int prescriptionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM PrescriptionItems WHERE PrescriptionId = @PrescriptionId";
                List<PrescriptionItemDTO> items = new List<PrescriptionItemDTO>();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                items.Add(new PrescriptionItemDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("PrescriptionId")),
                                     reader.GetString(reader.GetOrdinal("MedicineName")),
                                     reader.GetString(reader.GetOrdinal("Dosage")),
                                     reader.GetString(reader.GetOrdinal("Frequency")),
                                     reader.GetString(reader.GetOrdinal("Duration"))
                                 ));
                            }
                            if (items.Count() > 0)
                                return new Result<List<PrescriptionItemDTO>>(true, "Prescription Items found successfully", items);
                            else
                            {
                                return new Result<List<PrescriptionItemDTO>>(false, "No prescription item found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<PrescriptionItemDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<int>> UpdateAsync(PrescriptionItemDTO prescriptionItemDTO)
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

        public async Task<Result<bool>> DeleteAsync(int id)
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
