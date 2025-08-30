using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Authentication
{
    public class LoginResponseDTO
    {
        public LoginResponseDTO(string token, LoggedUserDTO user)
        {
            this.token = token;
            this.user = user;
        }

        public string token {  get; set; }
        public LoggedUserDTO user { get; set; }

    }
}
