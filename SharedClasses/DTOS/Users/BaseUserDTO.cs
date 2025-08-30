using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Users
{
    public class BaseUserDTO
    {
        public BaseUserDTO(string email, string userName)
        {
            this.email = email;
            this.userName = userName;
        }

        [Required(ErrorMessage = "This field is required!")]
        [EmailAddress(ErrorMessage ="Invalid email address!")]
        public string email {  get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [MinLength(5,ErrorMessage ="User name must be at least 5 char!")]
        public string userName { get; set; }

    }
}
