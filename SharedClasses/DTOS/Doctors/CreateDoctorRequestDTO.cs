using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class CreateDoctorRequestDTO
    {
        public CreateDoctorDTO dotorDTO {  get; set; }
        public CreateUserRequestDTO userDTO { get; set; }

        public CreateDoctorRequestDTO(CreateDoctorDTO dotorDTO, CreateUserRequestDTO userDTO)
        {
            this.dotorDTO = dotorDTO;
            this.userDTO = userDTO;
        }
    }
}
