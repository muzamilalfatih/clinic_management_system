using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.UserRoles;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class UserRoleRepository
    {
        private readonly string _connectionString;

        public UserRoleRepository ( IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<UserRoleDTO>> GetUserRoleInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM UserRoles WHERE Id = @id";
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
                                UserRoleDTO userRoleDTO = new UserRoleDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("RoleId")),
                                     reader.GetInt32(reader.GetOrdinal("UserId")),
                                     reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                     reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                                 );
                                return new Result<UserRoleDTO>(true, "UserRole found successfully", userRoleDTO);
                            }
                            else
                            {
                                return new Result<UserRoleDTO>(false, "UserRole not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<UserRoleDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> AddNewUserRoleAsync(CreateUserRoleDTO createUserRoleDTO, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO UserRoles
      (
      RoleId,
      UserId,
      IsActive
      )
VALUES
      (
      @RoleId
      ,@UserId
      ,@IsActive);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn,tran))
            {
                command.Parameters.AddWithValue("@RoleId", createUserRoleDTO.roleId);
                command.Parameters.AddWithValue("@UserId", createUserRoleDTO.userId);
                command.Parameters.AddWithValue("@IsActive", createUserRoleDTO.userId);
                object result = await command.ExecuteScalarAsync();
                int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (id > 0)
                {
                    return new Result<int>(true, "UserRole assign successfully.", id);
                }
                else
                {
                    return new Result<int>(false, "Failed to assign userRole.", -1, 500);
                }
               
            }
        }

        public async Task<Result<bool>> AddNewUserRoleAsync(CreateUserRoleDTO createUserRoleDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO UserRoles
      (
      RoleId,
      UserId,
      IsActive,
      IsComplete
      )
VALUES
      (
      @RoleId
      ,@UserId
      ,@IsActive
      ,@IsComplete);
SELECT SCOPE_IDENTITY();
";
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleId", createUserRoleDTO.roleId);
                    command.Parameters.AddWithValue("@UserId", createUserRoleDTO.userId);
                    command.Parameters.AddWithValue("@IsActive", createUserRoleDTO.userId);
                    command.Parameters.AddWithValue("@IsComplete", createUserRoleDTO.isComplete);
                    try
                    {
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<bool>(true, "UserRole assigned successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to assigned userRole.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public  async Task<Result<int>> UpdateUserRoleAsync(UserRoleDTO userRoleDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE UserRoles
SET 
    RoleId = @RoleId,
    UserId = @UserId
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", userRoleDTO.id);
                    command.Parameters.AddWithValue("@RoleId", userRoleDTO.roleId);
                    command.Parameters.AddWithValue("@UserId", userRoleDTO.userId);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "UserRole updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update userRole.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeleteUserRoleAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM UserRoles WHERE Id = @id";
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
                            return new Result<bool>(true, "UserRole deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete userRole.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<List<UserRoleDTO>>> GetAllUserRolesAsync(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT UserRoles.RoleId, UserRoles.UserId, UserRoles.IsActive, UserRoles.CreatedDate
FROM     Users INNER JOIN
                  UserRoles ON Users.Id = UserRoles.UserId
where Users.Email = @email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    List<UserRoleDTO> roles = new List<UserRoleDTO>();

                    try
                    {
                        await connection.OpenAsync();
                        using(SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                UserRoleDTO userRoleDTO = new UserRoleDTO(
                                    id:0,
                                    reader.GetInt32(reader.GetOrdinal("RoleId")),
                                    reader.GetInt32(reader.GetOrdinal("UserId")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                                    );
                                roles.Add(userRoleDTO);
                            }
                            return new Result<List<UserRoleDTO>>(true, "Roles retries successfuly!", roles);
                        }                       
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<UserRoleDTO>>(false, "An unexpected error occurred on the server.", roles, 500);
                    }

                }
            }
        }
        public async Task<Result<List<UserRoleDTO>>> GetAllUserRolesAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT UserRoles.RoleId, UserRoles.UserId, UserRoles.IsActive, UserRoles.CreatedDate
FROM     Users INNER JOIN
                  UserRoles ON Users.Id = UserRoles.UserId
where Users.Id = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    List<UserRoleDTO> roles = new List<UserRoleDTO>();

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                UserRoleDTO userRoleDTO = new UserRoleDTO(
                                    id: 0,
                                    reader.GetInt32(reader.GetOrdinal("RoleId")),
                                    reader.GetInt32(reader.GetOrdinal("UserId")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                                    );
                                roles.Add(userRoleDTO);
                            }
                            return new Result<List<UserRoleDTO>>(true, "Roles retries successfuly!", roles);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<UserRoleDTO>>(false, "An unexpected error occurred on the server.", roles, 500);
                    }

                }
            }
        }
        public async Task<Result<List<RolesForUserDTO>>> GetAllRolesInfoAsync (int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT UserRoles.RoleId, Roles.Name as Role, UserRoles.IsActive, UserRoles.CreatedDate, UserRoles.IsComplete
FROM     UserRoles INNER JOIN
                  Roles ON UserRoles.RoleId = Roles.Id
WHERE  (UserRoles.UserId = @UserId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    List<RolesForUserDTO> roles = new List<RolesForUserDTO>();

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                RolesForUserDTO userRoleDTO = new RolesForUserDTO(
                                    reader.GetInt32(reader.GetOrdinal("RoleId")),
                                    reader.GetString(reader.GetOrdinal("Role")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetBoolean(reader.GetOrdinal("IsComplete"))
                                    );
                                roles.Add(userRoleDTO);
                            }
                            return new Result<List<RolesForUserDTO>>(true, "Roles retries successfuly!", roles);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<RolesForUserDTO>>(false, "An unexpected error occurred on the server.", roles, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> ToggleStatusAsync(ToggleRoleStatusDTO toggleRoleStatusDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE UserRoles
SET 
    IsActive = @IsActive
WHERE where UserId = @UserId and RoleId = @RoleId
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsActive", toggleRoleStatusDTO.isActive);
                    command.Parameters.AddWithValue("@RoleId", toggleRoleStatusDTO.roleId);
                    command.Parameters.AddWithValue("@UserId", toggleRoleStatusDTO.userId);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            string successMessage = $"User {toggleRoleStatusDTO.userId} role {toggleRoleStatusDTO.roleId} Hass been " +
                                        $"{(toggleRoleStatusDTO.isActive ? "Activated" : "Deactivated")}";
                            return new Result<bool>(true, successMessage, true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Role not found.", false, 400);
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
