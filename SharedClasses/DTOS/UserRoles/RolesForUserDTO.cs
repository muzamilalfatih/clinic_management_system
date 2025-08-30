using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.UserRoles
{
    public class RolesForUserDTO
    {
        public RolesForUserDTO(int roleId, string role, bool isActive, DateTime createdDate, bool isComplete)
        {
            RoleId = roleId;
            Role = role;
            IsActive = isActive;
            CreatedDate = createdDate;
            IsComplete = isComplete;
        }

        public int RoleId {  get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsComplete { get; set; }

    }
}
