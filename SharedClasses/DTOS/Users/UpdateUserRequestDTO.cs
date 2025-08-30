using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class UpdateUserRequestDTO : BaseUserDTO
    {
        public UpdateUserRequestDTO(string email, string userName, string password)
            :base(email,userName)
            
        {
            this.password = password;
        }

        public string password { get; set; }

    }
}
