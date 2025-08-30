using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Doctors
{
    public class FullCreateDoctorResponseDTO
    {
        public FullCreateDoctorResponseDTO(int id, CreateDoctorResponseDTO doctorDTO, CreateUserResponseDTO UserDTO
            , CreatePersonResponseDTO person)
        {
            this.id = id;
            this.doctor = doctorDTO;
            this.User = UserDTO;
            this.person = person;
        }

        public int id {  get; set; }
        public CreateDoctorResponseDTO doctor { get; set; }
        public CreateUserResponseDTO User { get; set; }
        public CreatePersonResponseDTO person { get; set; }
    }
}
