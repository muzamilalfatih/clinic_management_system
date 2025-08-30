using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.UserRoles
{
    public class CreateUserRoleResquestDTO
    {
        public CreateUserRoleResquestDTO(string email, CreateUserRoleDTO createUserRoleDTO)
        {
            this.email = email;
            this.createUserRoleDTO = createUserRoleDTO;
        }

        public string email { get; set; }
        public CreateUserRoleDTO createUserRoleDTO { get; set; }

    }
}
