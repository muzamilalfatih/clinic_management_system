using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.People
{
    public class BasePersonDTO
    {
        public BasePersonDTO(string firstName, string secondName, string thirdName, string lastName,
            enGender gender, string phone, string address)
        {
            this.firstName = firstName;
            this.secondName = secondName;
            this.thirdName = thirdName;
            this.lastName = lastName;
            this.gender = gender;
            this.phone = phone;
            this.address = address;
        }

        [Required]
        [MinLength(3, ErrorMessage = "Min length is 3 char")]
        public string firstName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Min length is 3 char")]
        public string secondName { get; set; }
        [Required]
        public string thirdName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Min length is 3 char")]
        public string lastName { get; set; }
        [Required]
        public enGender gender { get; set; }
        [Required]
        [Phone(ErrorMessage = "Invalid phone format!")]
        public string phone { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Min length is 3 char")]
        public string address { get; set; }
    }
}
