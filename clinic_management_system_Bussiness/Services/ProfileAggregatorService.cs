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
        
        private Result<T> _createFailReponse<T>(ResponseMessage message, int code, T data)
        {
            return new Result<T>(false, message, data, code);
        }

        public async Task<Result<ResponseProfileDTO>> GetReponseProfileAsync(int userId)
        {
            Result<DoctorProfileDTO> doctorReult = null;
            Result<PatientProfileDTO> patientResult = null;
            Result<LabTechnicianProfileDTO> technicianProfileResult = null;


            Result<UserProfileDTO> userResult = await _userService.GetProfileAsync(userId);
            if (!userResult.Success)
                return _createFailReponse<ResponseProfileDTO>(userResult.Message, userResult.ErrorCode, null);

            foreach (UserRoleInfoDTO role in userResult.Data.roles)
            {
                if (role.roleName.Equals("Doctor",StringComparison.OrdinalIgnoreCase))
                {
                    doctorReult = await _doctorService.GetProfileAsync(userId);
                    if (!doctorReult.Success)
                        return _createFailReponse<ResponseProfileDTO>(doctorReult.Message, doctorReult.ErrorCode, null);
                }
                if (role.roleName.Equals("Patient", StringComparison.OrdinalIgnoreCase))
                {
                    patientResult = await _patientService.GetProfileAsync(userId);
                    if (!patientResult.Success)
                        return _createFailReponse<ResponseProfileDTO>(patientResult.Message, patientResult.ErrorCode, null);
                }
                if (role.roleName.Equals("LabTechnical", StringComparison.OrdinalIgnoreCase))
                {
                     technicianProfileResult = await _labTechnicianService.GetProfile(userId);
                    if (!technicianProfileResult.Success)
                        return _createFailReponse<ResponseProfileDTO>(technicianProfileResult.Message, technicianProfileResult.ErrorCode, null);

                }
            }
            

            Result<PersonProfileDTO> personResult = await _personService.GetProfileAsync(userId);
            if (!personResult.Success)
                return _createFailReponse<ResponseProfileDTO>(personResult.Message, personResult.ErrorCode, null);

            ResponseProfileDTO responseProfileDTO = new ResponseProfileDTO(userResult.Data, personResult.Data, patientResult?.Data, doctorReult?.Data,technicianProfileResult?.Data);
            return new Result<ResponseProfileDTO>(true, "Profile response", responseProfileDTO);
        }
    }
}
