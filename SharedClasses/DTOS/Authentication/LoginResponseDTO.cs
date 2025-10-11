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
            Token = token;
            User = user;
        }

        public string Token {  get; set; }
        public LoggedUserDTO User { get; set; }

    }
}
