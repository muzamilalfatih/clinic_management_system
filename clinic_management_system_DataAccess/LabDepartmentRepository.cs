using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class LabDepartmentRepository
    {
        private readonly string _connectionString;

        public LabDepartmentRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<LabDepartmentDTO>> GetLabDepartmentInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM LabDepartments WHERE Id = @id";
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
                                LabDepartmentDTO labDepartmentDTO = new LabDepartmentDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetString(reader.GetOrdinal("Name"))
                                 );
                                return new Result<LabDepartmentDTO>(true, "LabDepartment found successfully", labDepartmentDTO);
                            }
                            else
                            {
                                return new Result<LabDepartmentDTO>(false, "LabDepartment not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<LabDepartmentDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> AddNewLabDepartmentAsync(LabDepartmentDTO labDepartmentDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO LabDepartments
      (
      Name)
VALUES
      (
      @Name);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", labDepartmentDTO.name);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "LabDepartment added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add labDepartment.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> UpdateLabDepartmentAsync(LabDepartmentDTO labDepartmentDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE LabDepartments
SET 
    Name = @Name
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", labDepartmentDTO.id);
                    command.Parameters.AddWithValue("@Name", labDepartmentDTO.name);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "LabDepartment updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update labDepartment.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeleteLabDepartmentAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM LabDepartments WHERE Id = @id";
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
                            return new Result<bool>(true, "LabDepartment deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete labDepartment.", false);
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
