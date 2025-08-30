using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Patients
{
    public class CreatePatientRequestDTO
    {
        public CreatePatientRequestDTO(CreatePatientDTO patientDTO, CreateUserRequestDTO userDTO)
        {
            this.patientDTO = patientDTO;
            this.userDTO = userDTO;
        }

        public CreatePatientDTO patientDTO {  get; set; }
        public CreateUserRequestDTO userDTO { get; set; }
    }
}
