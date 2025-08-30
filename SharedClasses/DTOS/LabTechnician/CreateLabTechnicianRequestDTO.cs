using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTechnician
{
    public class CreateLabTechnicianRequestDTO
    {
        public CreateLabTechnicianRequestDTO(CreateLabTechnicianDTO labTechnicianDTO, CreateUserRequestDTO userDTO)
        {
            UserDTO = userDTO;
            LabTechnicianDTO = labTechnicianDTO;
        }

        public CreateLabTechnicianDTO LabTechnicianDTO { get; set; }
        public CreateUserRequestDTO UserDTO { get; set; }
    }
}
