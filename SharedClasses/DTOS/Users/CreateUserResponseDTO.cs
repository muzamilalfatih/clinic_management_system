using SharedClasses.DTOS.People;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class CreateUserResponseDTO : BaseUserDTO
    {
        public CreateUserResponseDTO(string userName, string email, Roles role, CreatePersonResponseDTO createPersonResponseDTO)
            :base(email, userName)
        {
            this.Role = role;
            PersonResponse = createPersonResponseDTO;
        }

        public Roles Role { get; set; }
        public CreatePersonResponseDTO PersonResponse { get; set; }

    }
}
