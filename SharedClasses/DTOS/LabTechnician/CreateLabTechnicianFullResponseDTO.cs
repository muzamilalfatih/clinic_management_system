using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTechnician
{
    public class CreateLabTechnicianFullResponseDTO
    {
        public CreateLabTechnicianFullResponseDTO(int id, CreateLabTechnicianResponseDTO labTechnicianResponseDTO, 
            CreateUserResponseDTO userResponseDTO, CreatePersonResponseDTO personResponseDTO)
        {
            Id = id;
            LabTechnicianResponseDTO = labTechnicianResponseDTO;
            UserResponseDTO = userResponseDTO;
            PersonResponseDTO = personResponseDTO;
        }

        public int Id { get; set; }
        public CreateLabTechnicianResponseDTO LabTechnicianResponseDTO { get; set; }
        public CreateUserResponseDTO UserResponseDTO { get; set; }
        public CreatePersonResponseDTO PersonResponseDTO { get; set; }
    }
}
