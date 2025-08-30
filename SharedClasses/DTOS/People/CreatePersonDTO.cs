using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.People
{
    public class CreatePersonDTO : BasePersonDTO
    {
        public CreatePersonDTO(string firstName, string secondName, string thirdName,
            string lastName, enGender gender, string phone, string address)
            :base(firstName, secondName, thirdName, lastName,gender,phone,address)
        {
            
        }
        
    }
}
