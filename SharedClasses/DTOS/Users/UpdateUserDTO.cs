using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class UpdateUserDTO : BaseUserDTO
    {
        public UpdateUserDTO(int id, string email, string userName, string password)
           :base(email, userName)
        {
            Id = id;
            Password = password;
        }
        public int Id { get; set; }
        public string Password { get; set; }
    }
}
