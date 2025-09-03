using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Appointment;
using SharedClasses.DTOS.LabTestParameter;
using SharedClasses.DTOS.LabTestParameter;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_DataAccess
{
    public class LabTestParameterRepository
    {
        private readonly string _connectionString;

        public LabTestParameterRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<LabTestParameterDTO>> GetLabTestParameterByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select * from LabTestParameters ";
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
                                LabTestParameterDTO LabTestParameterDTO = new LabTestParameterDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("LabTestId")),
                                     reader.GetString(reader.GetOrdinal("Name")),
                                     reader.GetString(reader.GetOrdinal("NormalRange")),
                                     reader.GetString(reader.GetOrdinal("Unit"))
                                 );
                                return new Result<LabTestParameterDTO>(true, "LabTestParameter found successfully", LabTestParameterDTO);
                            }
                            else
                            {
                                return new Result<LabTestParameterDTO>(false, "LabTestParameter not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabTestParameterDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<int>> AddNewLabTestParameterAsync(AddNewLabTestParameterDTO addNew)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO LabTestParameters
      (
      LabTestId
      ,Name
      ,NormalRange
      ,Unit)
VALUES
      (
      @LabTestId
      ,@Name
      ,@NormalRange
      ,@Unit

);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LabTestId", addNew.LabTestId);
                    command.Parameters.AddWithValue("@Name", addNew.Name);
                    command.Parameters.AddWithValue("@NormalRange", addNew.NormalRange);
                    command.Parameters.AddWithValue("@Unit", addNew.Unit ?? (object) DBNull.Value);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "LabTestParameter added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add LabTestParameter.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<int>> UpdateLabTestParameterAsync(UpdateLabTestParameterDTO update)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE LabTestParameters
SET 
    LabTestId = @LabTestId,
    Name = @Name,
    NormalRange = @NormalRange 
    Unit = @Unit

WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", update.Id);
                    command.Parameters.AddWithValue("@LabTestid", update.LabTestId);
                    command.Parameters.AddWithValue("@Name", update.Name);
                    command.Parameters.AddWithValue("@NormalRange", update.NormalRange);
                    command.Parameters.AddWithValue("@Unit", update.Unit ?? (object) DBNull.Value);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "LabTestParameter updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update LabTestParameter.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteLabTestParameterAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM LabTestParameters WHERE Id = @id";
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
                            return new Result<bool>(true, "LabTestParameter deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete LabTestParameter.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<List<LabTestParameterDTO>>> GetAllAsync(int labTestId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select * from LabTestparameters
                                where LabtestId = @LabTestId ;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LabTestId", labTestId);


                    List<LabTestParameterDTO> labTestParameters = new List<LabTestParameterDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            while (reader.Read())
                            {
                                labTestParameters.Add(new LabTestParameterDTO(
                                        reader.GetInt32(reader.GetOrdinal("Id")),
                                        reader.GetInt32(reader.GetOrdinal("LabTestId")),
                                        reader.GetString(reader.GetOrdinal("Name")),
                                        reader.GetString(reader.GetOrdinal("NormalRange")),
                                        reader.GetString(reader.GetOrdinal("Unit"))
                                    ));
                            }

                            return new Result<List<LabTestParameterDTO>>(true, "Appointments retrieved successfully", labTestParameters);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<LabTestParameterDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }

        }
        public async Task<Result<bool>> IsExistAsync(int labTestId, string name)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select Found = 1 from labTestparameters
                                 where LabTestId = @LabTestId and Name = @Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LabTestId", labTestId);
                    command.Parameters.AddWithValue("@Name", name);
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

        //        public async Task<Result<bool>> IsExist(int appointmentId)
        //        {
        //            using (SqlConnection connection = new SqlConnection(_connectionString))
        //            {
        //                string query = @"
        //select Found = 1 from LabTestParameters 
        //where AppoinmentId = @AppointmentId";
        //                using (SqlCommand command = new SqlCommand(query, connection))
        //                {
        //                    command.Parameters.AddWithValue("@AppointmentId", appointmentId);
        //                    bool isFound;
        //                    try
        //                    {
        //                        await connection.OpenAsync();
        //                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
        //                        {
        //                            isFound = reader.HasRows;
        //                        }
        //                        return new Result<bool>(true, "check completed.", isFound);

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
        //                    }

        //                }
        //            }
        //        }
    }
}
