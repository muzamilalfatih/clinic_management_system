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
        public LoggedUserDTO(int id, string dualName, string email, string userName, List<UserRoleInfoDTO> roles)
            :base(email, userName)
        {
            this.id = id;
            this.dualName = dualName;
            this.roles = roles;
            this.email = email;
        }

        public int id { get; set; }
        public string dualName { get; set; }
        public List<UserRoleInfoDTO> roles { get; set; }

    }
}
