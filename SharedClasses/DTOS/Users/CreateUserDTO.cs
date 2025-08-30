using SharedClasses.DTOS.People;
using SharedClasses.DTOS.UserRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class CreateUserDTO : BaseUserDTO
    {
        public CreateUserDTO( string email, string userName, 
            string password,  CreateUserRoleDTO createUserRoleDTO, int personId)
            :base(email, userName)
        {
            this.email = email;
            this.userName = userName;
            this.password = password;
            this.createUserRoleDTO = createUserRoleDTO;
            this.personId = personId;
        }
        [Required(ErrorMessage ="This field is required!")]
        [Range(1,int.MaxValue, ErrorMessage = "Invalid id!")]
        public int personId { get; set; }
        [MinLength(5, ErrorMessage = "At least 5 charecters")]
        [Required]
        public string password { get; set; }
        public CreateUserRoleDTO createUserRoleDTO { get; set; }
    }
}
