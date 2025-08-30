using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Receptionists
{
    public class CreateReceptionistRequestDTO
    {
        public CreateReceptionistRequestDTO(CreateReceptionistDTO receptionistDTO,
            CreateUserRequestDTO userDTO)
        {
            ReceptionistDTO = receptionistDTO;
            UserDTO = userDTO;
        }

        public CreateReceptionistDTO ReceptionistDTO {  get; set; }
        public CreateUserRequestDTO UserDTO {  get; set; }
    }
}
