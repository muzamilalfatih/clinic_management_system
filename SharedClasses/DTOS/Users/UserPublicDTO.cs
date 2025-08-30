using SharedClasses.DTOS.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class UserPublicDTO : BaseUserDTO
    {
        public UserPublicDTO(UserDTO user)
            :base(user.email,user.userName)
        {
            this.id = user.id;
            this.roles = user.roles;
        }
        public int id { get; set; }
        public List<UserRoleInfoDTO> roles { get; set; }
    }
}
