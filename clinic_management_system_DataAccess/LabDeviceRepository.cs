using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Bills;
using SharedClasses.DTOS.Doctors;
using SharedClasses.DTOS.LabDevices;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_DataAccess
{

    public class LabDeviceRepository
    {
    private readonly string _connectionString;

        public LabDeviceRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;

        }

        public async Task<Result<LabDeviceDTO>> GetLabDeviceAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM LabDevices WHERE Id = @id";
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
                                LabDeviceDTO labDeviceDTO = new LabDeviceDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetString(reader.GetOrdinal("Name")),
                                     reader.GetString(reader.GetOrdinal("Model")),
                                     reader.GetString(reader.GetOrdinal("ConnectionType")),
                                     reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                 );
                                return new Result<LabDeviceDTO>(true, "Bill found successfully", labDeviceDTO);
                            }
                            else
                            {
                                return new Result<LabDeviceDTO>(false, "Bill not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabDeviceDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<LabDeviceDTO>> GetLabDeviceAsync(string name)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM LabDevices WHERE Name = @Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                LabDeviceDTO labDeviceDTO = new LabDeviceDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetString(reader.GetOrdinal("Name")),
                                     reader.GetString(reader.GetOrdinal("Model")),
                                     reader.GetString(reader.GetOrdinal("ConnectionType")),
                                     reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                 );
                                return new Result<LabDeviceDTO>(true, "Lab device found successfully", labDeviceDTO);
                            }
                            else
                            {
                                return new Result<LabDeviceDTO>(false, "Lab device not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabDeviceDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<List<LabDeviceDTO>>> GetAllAsync(int pageNumber, int pageSize)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                select * from LabDevices 
                order by Name
                OFFSET (@PageNumber - 1) * @PageSize ROWS
                FETCH NEXT @PageSize ROWS ONLY;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    List<LabDeviceDTO> labDevices = new List<LabDeviceDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                labDevices.Add(new LabDeviceDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetString(reader.GetOrdinal("Model")),
                                    reader.GetString(reader.GetOrdinal("ConnectionType")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                ));
                            }
                            if (labDevices.Count() > 0)
                                return new Result<List<LabDeviceDTO>>(true, "LabDevices retrieved successfully.", labDevices);
                            else
                                return new Result<List<LabDeviceDTO>>(false, "No lab devices found!", labDevices);

                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<LabDeviceDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<int>> AddNewLabDeviceAsync(CreateLabDeviceDTO createLabDeviceDTO )
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO LabDevices
      (
      Name,
      Model,
      ConnectionType,
      IsActive)
)
VALUES
      (
      @Name,
      @Model,
      @ConnectionType,
      @IsActive);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@Name", createLabDeviceDTO.Name);
                    command.Parameters.AddWithValue("@Model", createLabDeviceDTO.Model);
                    command.Parameters.AddWithValue("@ConnectionType", createLabDeviceDTO.ConnectionType);
                    command.Parameters.AddWithValue("@IsActive", createLabDeviceDTO.IsActive);


                    object? result = await command.ExecuteScalarAsync();
                    int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    if (id > 0)
                    {
                        return new Result<int>(true, "LabDevice added successfully.", id);
                    }
                    else
                    {
                        return new Result<int>(false, "Failed to add LabDevice.", -1);
                    }

                }
            }
        }
        public async Task<Result<bool>> UpdateDoctorAsync(UpdateLabDeviceDTO updateLabDeviceDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Doctors
SET 
    Name = @Name,
    Model = @Model,  
    ConnectionType = @ConnectionType,
    IsActvie = @IsActvie

WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", updateLabDeviceDTO.Id);
                    command.Parameters.AddWithValue("@Name", updateLabDeviceDTO.Name);
                    command.Parameters.AddWithValue("@Model", updateLabDeviceDTO.Model);
                    command.Parameters.AddWithValue("@ConnectionType", updateLabDeviceDTO.ConnectionType);
                    command.Parameters.AddWithValue("@IsActvie", updateLabDeviceDTO.IsActive);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "lab Device updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Lab Device not found.", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<int>> GetIdAsync(string name)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Id FROM LabDevices 
WHERE Name = @Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "LabDevice id retrieved successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "LabDevice not found.", -1, 404);
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
