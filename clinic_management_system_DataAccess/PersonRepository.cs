using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.People;
using System.Data;
namespace clinic_management_system_DataAccess
{
    public class PersonRepository
    {
        private readonly string _connectionString;

        public PersonRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public  async Task<Result<PersonDTO>> GetPersonInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM People WHERE Id = @id";
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
                                PersonDTO personDTO = new PersonDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetString(reader.GetOrdinal("FirstName")),
                                     reader.GetString(reader.GetOrdinal("SecondName")),
                                     reader.GetString(reader.GetOrdinal("ThirdName")),
                                     reader.GetString(reader.GetOrdinal("LastName")),
                                     (enGender)reader.GetByte(reader.GetOrdinal("Gender")),
                                     reader.GetString(reader.GetOrdinal("Phone")),
                                     reader.GetString(reader.GetOrdinal("Address"))
                                 );
                                return new Result<PersonDTO>(true, "Person found successfully", personDTO);
                            }
                            else
                            {
                                return new Result<PersonDTO>(false, "Person not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PersonDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<PersonProfileDTO>> GetProfileAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT People.FirstName,People.SecondName, People.ThirdName, People.LastName, People.Gender, People.Phone, People.Address
FROM     People INNER JOIN
                  Users ON People.Id = Users.PersonId
where Users.Id = @UserId";
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
                                PersonProfileDTO personProfileDTO = new PersonProfileDTO
                                 (
                                     reader.GetString(reader.GetOrdinal("FirstName")),
                                     reader.GetString(reader.GetOrdinal("SecondName")),
                                     reader.GetString(reader.GetOrdinal("ThirdName")),
                                     reader.GetString(reader.GetOrdinal("LastName")),
                                     (enGender)reader.GetByte(reader.GetOrdinal("Gender")),
                                     reader.GetString(reader.GetOrdinal("Phone")),
                                     reader.GetString(reader.GetOrdinal("Address"))
                                 );
                                return new Result<PersonProfileDTO>(true, "Person found successfully", personProfileDTO);
                            }
                            else
                            {
                                return new Result<PersonProfileDTO>(false, "Person not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PersonProfileDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }

        }

        public  async Task<Result<int>> AddNewPersonAsync(CreatePersonDTO createPersonDTO, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
INSERT INTO People
      (
      FirstName
      ,SecondName
      ,ThirdName
      ,LastName
      ,Gender
      ,Phone
      ,Address)
VALUES
      (
      @FirstName
      ,@SecondName
      ,@ThirdName
      ,@LastName
      ,@Gender
      ,@Phone
      ,@Address);
SELECT SCOPE_IDENTITY();
";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@FirstName", createPersonDTO.firstName);
                command.Parameters.AddWithValue("@SecondName", createPersonDTO.secondName);
                command.Parameters.AddWithValue("@ThirdName", createPersonDTO.thirdName);
                command.Parameters.AddWithValue("@LastName", createPersonDTO.lastName);
                command.Parameters.AddWithValue("@Gender", (byte)createPersonDTO.gender);
                command.Parameters.AddWithValue("@Phone", createPersonDTO.phone);
                command.Parameters.AddWithValue("@Address", createPersonDTO.address);

                object result = await command.ExecuteScalarAsync();
                int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (id > 0)
                {
                    return new Result<int>(true, "Person added successfully.", id);
                }
                else
                {
                    return new Result<int>(false, "Failed to add Person.", -1, 500);

                }
            }
        }

        public  async Task<Result<bool>> UpdatePersonAsync(UpdatePersonDTO updatePersonDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE People
SET 
    FirstName = @FirstName,
    SecondName = @SecondName,
    ThirdName = @ThirdName,
    LastName = @LastName,
    Gender = @Gender,
    Phone = @Phone,
    Address = @Address
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", updatePersonDTO.id);
                    command.Parameters.AddWithValue("@FirstName", updatePersonDTO.firstName);
                    command.Parameters.AddWithValue("@SecondName", updatePersonDTO.secondName);
                    command.Parameters.AddWithValue("@ThirdName", updatePersonDTO.thirdName);
                    command.Parameters.AddWithValue("@LastName", updatePersonDTO.lastName);
                    command.Parameters.AddWithValue("@Gender", (byte)updatePersonDTO.gender);
                    command.Parameters.AddWithValue("@Phone", updatePersonDTO.phone);
                    command.Parameters.AddWithValue("@Address", updatePersonDTO.address);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Person updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Person not found.", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public  async Task<Result<bool>> DeletePersonAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM People WHERE Id = @id";
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
                            return new Result<bool>(true, "Person deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete person.", false);
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
