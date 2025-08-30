using SharedClasses.DTOS.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class UserProfileDTO : BaseUserDTO
    {
        public UserProfileDTO(int id, string userName, string email, List<UserRoleInfoDTO> roles) 
            :base(email, userName)
        {
            this.id = id;
            this.roles = roles;
        }  
        public int id {  get; set; }
        public List<UserRoleInfoDTO> roles { get; set; }

    }
}
