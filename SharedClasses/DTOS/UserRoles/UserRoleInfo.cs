using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.UserRoles
{
    public class UserRoleInfoDTO
    {
        public UserRoleInfoDTO(string roleName, bool isActive)
        {
            this.roleName = roleName;
            this.isActive = isActive;
        }
        public string roleName {  get; set; }
        public bool isActive { get; set; }

    }
}
