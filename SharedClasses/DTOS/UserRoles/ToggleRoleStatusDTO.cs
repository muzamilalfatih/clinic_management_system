using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.UserRoles
{
    public class ToggleRoleStatusDTO
    {
        public ToggleRoleStatusDTO(int userId, int roleId, bool isActive)
        {
            this.userId = userId;
            this.roleId = roleId;
            this.isActive = isActive;
        }

        public int userId {  get; set; }
        public int roleId {  get; set; }
        public bool isActive { get; set; }   
    }
}
