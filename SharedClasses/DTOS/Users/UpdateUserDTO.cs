using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class UpdateUserDTO : BaseUserDTO
    {
        public UpdateUserDTO(int id, UpdateUserRequestDTO updateUserRequestDTO)
           :base(updateUserRequestDTO.email, updateUserRequestDTO.userName)
        {
            this.id = id;
            this.password = updateUserRequestDTO.password;
        }
        public int id { get; set; }
        public string password { get; set; }
    }
}
