using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.UserRoles
{
    public class CreateUserRoleDTO
    {
        public CreateUserRoleDTO(int roleId, int userId, bool isActive = true)
        {
            this.roleId = roleId;
            this.userId = userId;
            this.isActive = isActive;
        }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than zero")]
        public int roleId {  get; set; }
        public int userId { get; set; }
        [Required]
        public bool isActive { get; set; }
        public bool isComplete { get; set; }

    }
}
