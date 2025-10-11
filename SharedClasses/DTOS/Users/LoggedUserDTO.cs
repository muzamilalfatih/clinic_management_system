using SharedClasses.DTOS.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class LoggedUserDTO : BaseUserDTO
    {
        public LoggedUserDTO(int id, int personId, string email, string userName, List<UserRoleInfoDTO> roles)
            :base(email, userName)
        {
            Id = id;
            PersonId = personId;
            Roles = roles;
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public List<UserRoleInfoDTO> Roles { get; set; }

    }
}
