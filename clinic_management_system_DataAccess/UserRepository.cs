using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.UserRoles;
using SharedClasses.DTOS.Users;
using System.Data;
using System.Data.Common;
namespace clinic_management_system_DataAccess
{
    public class UserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<UserDTO>> GetUserInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Users.*, Roles.Name, UserRoles.IsActive
FROM     Users INNER JOIN
                  UserRoles ON Users.Id = UserRoles.UserId INNER JOIN
                  Roles ON UserRoles.RoleId = Roles.Id
where Users.id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    UserDTO userDTO = null;
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (userDTO == null)
                                {
                                    userDTO = new UserDTO
                                            (
                                                reader.GetInt32(reader.GetOrdinal("Id")),
                                                reader.GetInt32(reader.GetOrdinal("PersonId")),
                                                reader.GetString(reader.GetOrdinal("Email")),
                                                reader.GetString(reader.GetOrdinal("UserName")),
                                                reader.GetString(reader.GetOrdinal("Password")),
                                                  new List<UserRoleInfoDTO>()
                                            );
                                }
                                userDTO.roles.Add(new UserRoleInfoDTO(
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive")))
                                    );
                            }
                            return new Result<UserDTO>(true, "User found successfully", userDTO);
                            //return new Result<UserDTO>(false, "User not found.", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<UserDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<UserProfileDTO>> GetProfileAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Users.Id,Users.Email,UserName, Roles.Name, UserRoles.IsActive
FROM     Users INNER JOIN
                  UserRoles ON Users.Id = UserRoles.UserId INNER JOIN
                  Roles ON UserRoles.RoleId = Roles.Id
where Users.id = @UserId
";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    UserProfileDTO userProfileDTO = null;
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (userProfileDTO == null)
                                {
                                    userProfileDTO = new UserProfileDTO
                                     (
                                        reader.GetInt32(reader.GetOrdinal("Id")),
                                         reader.GetString(reader.GetOrdinal("Email")),
                                         reader.GetString(reader.GetOrdinal("UserName")),
                                         new List<UserRoleInfoDTO>()
                                     );
                                }
                                userProfileDTO.roles.Add(new UserRoleInfoDTO(
                                        reader.GetString(reader.GetOrdinal("Name")),
                                        reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                    ));

                            }
                            return new Result<UserProfileDTO>(true, "User found successfully", userProfileDTO);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<UserProfileDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<UserDTO>> GetUserInfoByEmailAsync(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Users.*, Roles.Name, UserRoles.IsActive
FROM     Users INNER JOIN
                  UserRoles ON Users.Id = UserRoles.UserId INNER JOIN
                  Roles ON UserRoles.RoleId = Roles.Id
where Users.Email = @email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            UserDTO userDTO = null;
                            while (await reader.ReadAsync())
                            {
                                if (userDTO == null)
                                {
                                    userDTO = new UserDTO
                                                 (
                                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                                     reader.GetInt32(reader.GetOrdinal("PersonId")),
                                                     reader.GetString(reader.GetOrdinal("Email")),
                                                     reader.GetString(reader.GetOrdinal("UserName")),
                                                     reader.GetString(reader.GetOrdinal("Password")),
                                                     new List<UserRoleInfoDTO>()

                                                 );
                                }

                                userDTO.roles.Add(new UserRoleInfoDTO(
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                    ));
                            }
                            if (userDTO == null)    
                                return new Result<UserDTO>(false, "User not found.", null, 404);
                            return new Result<UserDTO>(true,"User found", userDTO); 
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<UserDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<int>> AddNewUserAsync(CreateUserDTO createUserDTO, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO Users
      (
      PersonId
      ,Email
      ,UserName
      ,Password)
VALUES
      (
      @PersonId
      ,@Email
      ,@UserName
      ,@Password);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@PersonId", createUserDTO.personId);
                command.Parameters.AddWithValue("@Email", createUserDTO.email);
                command.Parameters.AddWithValue("@UserName", createUserDTO.userName);
                command.Parameters.AddWithValue("@Password", createUserDTO.password);

                object result = await command.ExecuteScalarAsync();
                int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (id > 0)
                {
                    return new Result<int>(true, "User added successfully.", id);
                }
                else
                {
                    return new Result<int>(false, "Failed to add user.", -1, 500);
                }          
            }
        }
        public async Task<Result<bool>> UpdateUserAsync(UpdateUserDTO updateUserDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Users
SET 
    Email = @Email,
    UserName = @UserName,
    Password = @Password
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Id", updateUserDTO.Id);
                    command.Parameters.AddWithValue("@Email", updateUserDTO.email);
                    command.Parameters.AddWithValue("@UserName", updateUserDTO.userName);
                    command.Parameters.AddWithValue("@Password", updateUserDTO.Password);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "User updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "User not found.", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> DeleteUserAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Users WHERE Id = @id";
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
                            return new Result<bool>(true, "User deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete user.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> IsUserExistByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT ID FROM Users WHERE Email = @email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    bool isFound;
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            isFound = reader.HasRows;
                        }
                        return new Result<bool>(true, "User existence check completed.", isFound);

                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> IsUserExistByUserName(string userName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT ID FROM Users WHERE UserName = @userName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userName", userName);
                    bool isFound;
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            isFound = reader.HasRows;
                        }
                        return new Result<bool>(true, "User existence check completed.", isFound);

                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<int>> GetPersonIdAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonId FROM Users 
WHERE Id = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);           
                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Person id retrieved successfully.", id);
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
