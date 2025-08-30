using SharedClasses.DTOS.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class CreateUserRequestDTO
    {
        public CreateUserRequestDTO(CreateUserDTO createUserDTO, CreatePersonDTO createPersonDTO)
        {
            CreateUserDTO = createUserDTO;
            CreatePersonDTO = createPersonDTO;
        }

        public CreateUserDTO CreateUserDTO { get; set; }
        public CreatePersonDTO CreatePersonDTO { get; set; }

    }
}
