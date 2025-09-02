using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SharedClasses;
using SharedClasses.DTOS.Diagnose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_DataAccess
{
    public class DiagnoseRepository
    {
        private readonly string _connectionString;

        public DiagnoseRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<DiagnoseInfoDTO>> GetDiagnoseInfoByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Diagnoses.Id, Diagnoses.AppoinmentId, People.FirstName + ' ' + People.SecondName+ ' ' + People.ThirdName+ ' ' + People.LastName as PatientName, Diagnoses.Date, Diagnoses.DiagnosisCode, Diagnoses.Description
FROM     Appointments INNER JOIN
                  Patients ON Appointments.PatientId = Patients.Id INNER JOIN
                  Diagnoses ON Appointments.Id = Diagnoses.AppoinmentId INNER JOIN
                  People ON Appointments.Id = People.Id";
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
                                DiagnoseInfoDTO DiagnoseDTO = new DiagnoseInfoDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("Id")),
                                     reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                                     reader.GetString(reader.GetOrdinal("PatientName")),
                                     reader.GetDateTime(reader.GetOrdinal("Date")),
                                     reader.GetString(reader.GetOrdinal("DiagnosisCode")),
                                     reader.GetString(reader.GetOrdinal("Description"))
                                 );
                                return new Result<DiagnoseInfoDTO>(true, "Diagnose found successfully", DiagnoseDTO);
                            }
                            else
                            {
                                return new Result<DiagnoseInfoDTO>(false, "Diagnose not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<DiagnoseInfoDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<int>> AddNewDiagnoseAsync(AddNewDiagnoseDTO addNew)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO Diagnoses
      (
      AppointmentId
      ,DiagnosisCode
      ,Description)
VALUES
      (
      @AppointmentId
      ,@DiagnosisCode
      ,@Description);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AppointmentId", addNew.AppointmentId);
                    command.Parameters.AddWithValue("@DiagnosisCode", addNew.DiagnosisCode);
                    command.Parameters.AddWithValue("@Description", addNew.Description);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Diagnose added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add Diagnose.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<int>> UpdateDiagnoseAsync(UpdateDiagnoseDTO update)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Diagnoses
SET 
    AppointmentId = @AppointmentId,
    DiagnosisCode = @DiagnosisCode,
    Description = @Description
WHERE Id = @Id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", update.Id);
                    command.Parameters.AddWithValue("@AppointmentId", update.AppointmentId);
                    command.Parameters.AddWithValue("@DiagnosisCode", update.DiagnosisCode);
                    command.Parameters.AddWithValue("@Description", update.Description);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<int>(true, "Diagnose updated successfully.", rowAffected);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to update Diagnose.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteDiagnoseAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Diagnoses WHERE Id = @id";
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
                            return new Result<bool>(true, "Diagnose deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete Diagnose.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> IsExist(int appointmentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
select Found = 1 from Diagnoses 
where AppoinmentId = @AppointmentId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    bool isFound;
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            isFound = reader.HasRows;
                        }
                        return new Result<bool>(true, "check completed.", isFound);

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
