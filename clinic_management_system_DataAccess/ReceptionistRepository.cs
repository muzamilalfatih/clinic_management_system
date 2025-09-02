using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Receptionists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_DataAccess
{
    public class ReceptionistRepository
    {
        private readonly string _connectionString;
        public ReceptionistRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }

        public async Task<Result<ReceptionistDTO>> GetReceptionistInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Receptionists WHERE Id = @id";
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
                                ReceptionistDTO ReceptionistDTO = new ReceptionistDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("UserId")),
                                     reader.GetInt32(reader.GetOrdinal("ShiftTypeId")),
                                     reader.GetDateTime(reader.GetOrdinal("HireDate"))
                                 );
                                return new Result<ReceptionistDTO>(true, "Receptionist found successfully", ReceptionistDTO);
                            }
                            else
                            {
                                return new Result<ReceptionistDTO>(false, "Receptionist not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<ReceptionistDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
//        public async Task<Result<UpdateReceptionistDTO>> GetOldDataAsync(int id)
//        {
//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                string query = @"SELECT Id, ShiftTypeId, HireDate FROM     Receptionists
//WHERE  (Id = @id)";
//                using (SqlCommand command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@id", id);

//                    try
//                    {
//                        await connection.OpenAsync();
//                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
//                        {
//                            if (await reader.ReadAsync())
//                            {
//                                UpdateReceptionistDTO ReceptionistDTO = new UpdateReceptionistDTO
//                                 (
//                                     reader.GetInt32(reader.GetOrdinal("Id")),
//                                     reader.GetInt32(reader.GetOrdinal("ShiftTypeId")),
//                                     reader.GetDateTime(reader.GetOrdinal("HireDate"))
//                                 );
//                                return new Result<UpdateReceptionistDTO>(true, "Receptionist found successfully", ReceptionistDTO);
//                            }
//                            else
//                            {
//                                return new Result<UpdateReceptionistDTO>(false, "Receptionist not found.", null, 404);
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        return new Result<UpdateReceptionistDTO>(false, "An unexpected error occurred on the server.", null, 500);
//                    }

//                }
//            }
//        }
        public async Task<Result<int>> AddNewReceptionistAsync(CreateReceptionistDTO createReceptionistDTO, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO Receptionists
      (
      UserId
      ,ShiftTypeId
      ,HireDate )
VALUES
      (
      @UserId
      ,@ShiftTypeId
      ,@HireDate
        );
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@UserId", createReceptionistDTO.UserId);
                command.Parameters.AddWithValue("@ShiftTypeId", createReceptionistDTO.ShiftTypeId);
                command.Parameters.AddWithValue("@HireDate", createReceptionistDTO.HireDate);

                object result = await command.ExecuteScalarAsync();
                int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (id > 0)
                {
                    return new Result<int>(true, "Receptionist added successfully.", id);
                }
                else
                {
                    return new Result<int>(false, "Failed to add receptionist!", 500);
                }               
            }
        }
        public async Task<Result<bool>> UpdateReceptionistAsync(UpdateReceptionistDTO updateReceptionistDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Receptionists
SET 
    ShiftTypeId = @ShiftTypeId,
    HireDate = @HireDate

WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", updateReceptionistDTO.Id);
                    command.Parameters.AddWithValue("@ShiftTypeId", updateReceptionistDTO.ShiftTypeId);
                    command.Parameters.AddWithValue("@HireDate", updateReceptionistDTO.HireDate);

                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Receptionist updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Receptionist not found.", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<ReceptionistProfileDTO>> GetProfileAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT ShiftTypes.Name as ShiftType, Receptionists.HireDate
FROM     Receptionists INNER JOIN
                  ShiftTypes ON Receptionists.ShiftTypeId = ShiftTypes.Id
WHERE Receptionists.UserId = @UserId;";
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
                                ReceptionistProfileDTO ReceptionistProfileDTO = new ReceptionistProfileDTO
                                 (
                                     reader.GetString(reader.GetOrdinal("ShiftType")),
                                     reader.GetDateTime(reader.GetOrdinal("HireDate"))
                                 );
                                return new Result<ReceptionistProfileDTO>(true, "Receptionist found successfully", ReceptionistProfileDTO);
                            }
                            else
                            {
                                return new Result<ReceptionistProfileDTO>(false, "Receptionist not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<ReceptionistProfileDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }

        }
        public async Task<Result<bool>> DeleteReceptionistAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Receptionists WHERE Id = @id";
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
                            return new Result<bool>(true, "Receptionist deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete Receptionist.", false);
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
select Id from Receptionists
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
                            return new Result<int>(true, "receptionist id retrieved successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "receptionist not found.", -1, 404);
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
