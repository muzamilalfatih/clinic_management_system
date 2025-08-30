using SharedClasses.DTOS.Authentication;
using SharedClasses.DTOS.Doctors;
using SharedClasses.DTOS.Patients;
using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClasses.DTOS.UserRoles;
using SharedClasses.DTOS.LabTechnician;

namespace clinic_management_system_Bussiness
{
    public  class ProfileAggregatorService
    {
        private readonly UserService _userService;
        private readonly DoctorService _doctorService;
        private readonly PatientService _patientService;
        private readonly PersonService _personService;
        private readonly LabTechnicianService _labTechnicianService;

        public ProfileAggregatorService(UserService userService, DoctorService doctorService, 
            PatientService patientService, PersonService personService, LabTechnicianService labTechnicianService)
        {
            _userService = userService;
            _doctorService = doctorService;
            _patientService = patientService;
            _personService = personService;
            _labTechnicianService = labTechnicianService;
        }
        private Result<T> _createFailReponse<T>(string message, int code, T data)
        {
            return new Result<T>(false, message, data, code);
        }
        public async Task<Result<ResponseProfileDTO>> GetReponseProfileAsync(int userId)
        {
            Result<DoctorProfileDTO> doctorReult = null;
            Result<PatientProfileDTO> patientResult = null;
            Result<LabTechnicianProfileDTO> technicianProfileResult = null;


            Result<UserProfileDTO> userResult = await _userService.GetProfileAsync(userId);
            if (!userResult.success)
                return _createFailReponse<ResponseProfileDTO>(userResult.message, userResult.errorCode, null);

            foreach (UserRoleInfoDTO role in userResult.data.roles)
            {
                if (role.roleName.Equals("Doctor",StringComparison.OrdinalIgnoreCase))
                {
                    doctorReult = await _doctorService.GetProfileAsync(userId);
                    if (!doctorReult.success)
                        return _createFailReponse<ResponseProfileDTO>(doctorReult.message, doctorReult.errorCode, null);
                }
                if (role.roleName.Equals("Patient", StringComparison.OrdinalIgnoreCase))
                {
                    patientResult = await _patientService.GetProfileAsync(userId);
                    if (!patientResult.success)
                        return _createFailReponse<ResponseProfileDTO>(patientResult.message, patientResult.errorCode, null);
                }
                if (role.roleName.Equals("LabTechnical", StringComparison.OrdinalIgnoreCase))
                {
                     technicianProfileResult = await _labTechnicianService.GetProfile(userId);
                    if (!technicianProfileResult.success)
                        return _createFailReponse<ResponseProfileDTO>(technicianProfileResult.message, technicianProfileResult.errorCode, null);

                }
            }
            

            Result<PersonProfileDTO> personResult = await _personService.GetProfileAsync(userId);
            if (!personResult.success)
                return _createFailReponse<ResponseProfileDTO>(personResult.message, personResult.errorCode, null);

            ResponseProfileDTO responseProfileDTO = new ResponseProfileDTO(userResult.data, personResult.data, patientResult?.data, doctorReult?.data,technicianProfileResult?.data);
            return new Result<ResponseProfileDTO>(true, "Profile response", responseProfileDTO);
        }
    }
}
