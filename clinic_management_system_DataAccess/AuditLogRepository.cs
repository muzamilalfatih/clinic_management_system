using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class AuditLogRepository
    {
        private readonly string _connectionString;
        public AuditLogRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<AuditLogDTO>> GetAuditLogInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM AuditLogs WHERE Id = @id";
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
                                AuditLogDTO auditLogDTO = new AuditLogDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetString(reader.GetOrdinal("EntityName")),
                                     reader.GetInt32(reader.GetOrdinal("EntityId")),
                                     reader.GetString(reader.GetOrdinal("Action")),
                                     reader.GetInt32(reader.GetOrdinal("PerformedBy")),
                                     reader.GetDateTime(reader.GetOrdinal("PerformedAt")),
                                     reader.GetValue(reader.GetOrdinal("OldValues")),
                                     reader.GetValue(reader.GetOrdinal("newValues"))
                                 );
                                return new Result<AuditLogDTO>(true, "AuditLog found successfully", auditLogDTO);
                            }
                            else
                            {
                                return new Result<AuditLogDTO>(false, "AuditLog not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<AuditLogDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> AddNewAuditLogAsync(CreateAuditLogDTO createAuditLogDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO AuditLogs
      (
      EntityName
      ,EntityId
      ,Action
      ,PerformedBy
      ,PerformedAt
      ,OldValues
      ,newValues)
VALUES
      (
      @EntityName
      ,@EntityId
      ,@Action
      ,@PerformedBy
      ,@PerformedAt
      ,@OldValues
      ,@newValues);
SELECT SCOPE_IDENTITY();
";
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EntityName", createAuditLogDTO.entityName);
                    command.Parameters.AddWithValue("@EntityId", createAuditLogDTO.entityId);
                    command.Parameters.AddWithValue("@Action", createAuditLogDTO.action);
                    command.Parameters.AddWithValue("@PerformedBy", createAuditLogDTO.performedBy);
                    if (!string.IsNullOrWhiteSpace(createAuditLogDTO.oldValue))
                        command.Parameters.AddWithValue("@OldValues", createAuditLogDTO.oldValue);
                    else
                        command.Parameters.AddWithValue("@OldValues", DBNull.Value);
                    if (!string.IsNullOrWhiteSpace(createAuditLogDTO.newValue))
                        command.Parameters.AddWithValue("@newValues", createAuditLogDTO.newValue);
                    else
                        command.Parameters.AddWithValue("@newValues", DBNull.Value);


                    try
                    {
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<bool>(true, "Action log successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to log action", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
            
        }

        public async Task<Result<int>> UpdateAuditLogAsync(AuditLogDTO auditLogDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE AuditLogs
SET 
    EntityName = @EntityName,
    EntityId = @EntityId,
    Action = @Action,
    PerformedBy = @PerformedBy,
    PerformedAt = @PerformedAt,
    OldValues = @OldValues,
    newValues = @newValues
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", auditLogDTO.id);
                    command.Parameters.AddWithValue("@EntityName", auditLogDTO.entityName);
                    command.Parameters.AddWithValue("@EntityId", auditLogDTO.entityId);
                    command.Parameters.AddWithValue("@Action", auditLogDTO.action);
                    command.Parameters.AddWithValue("@PerformedBy", auditLogDTO.performedBy);
                    command.Parameters.AddWithValue("@PerformedAt", auditLogDTO.performedAt);
                    command.Parameters.AddWithValue("@OldValues", auditLogDTO.oldValues);
                    command.Parameters.AddWithValue("@newValues", auditLogDTO.newValues);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "AuditLog updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update auditLog.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteAuditLogAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM AuditLogs WHERE Id = @id";
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
                            return new Result<bool>(true, "AuditLog deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete auditLog.", false);
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
