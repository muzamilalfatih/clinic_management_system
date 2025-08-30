using SharedClasses.DTOS.UserRoles;
using SharedClasses.DTOS.Users;

namespace SharedClasses
{
    public class UserDTO : BaseUserDTO
     {
        public int id { get; set; }
        public int personId { get; set; }
        public string password { get; set; } 
        public List<UserRoleInfoDTO> roles { get; set; }
        public UserDTO(int id, int personId, string email, string userName, string password,List<UserRoleInfoDTO> roles)
                :base(email,userName)
         {
             this.id = id;
             this.personId = personId;
             this.password = password;
            this.roles = roles;
        }
     }
}
