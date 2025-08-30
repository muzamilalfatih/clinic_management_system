using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Receptionists
{
    public class FullCreateReceptionistResponseDTO
    {
        public FullCreateReceptionistResponseDTO(int id, CreateReceptionistResponseDTO receptionist, CreateUserResponseDTO userDTO,
            CreatePersonResponseDTO personDTO)
        {
            Id = id;
            Receptionist = receptionist;
            UserDTO = userDTO;
            PersonDTO = personDTO;
        }

        public int Id { get; set; }
        public CreateReceptionistResponseDTO Receptionist { get; set; }
        public CreateUserResponseDTO UserDTO { get; set; }
        public CreatePersonResponseDTO PersonDTO { get; set; }

    }
}
