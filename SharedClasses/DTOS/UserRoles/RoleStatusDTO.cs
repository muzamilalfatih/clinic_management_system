using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.UserRoles
{
    public class RoleStatusDTO
    {
        public RoleStatusDTO(bool isActive)
        {
            this.isActive = isActive;
        }

        public bool isActive { get; set; }

    }
}
