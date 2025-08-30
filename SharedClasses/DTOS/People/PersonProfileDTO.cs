using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.People
{
    public class PersonProfileDTO :BasePersonDTO
    {
        public PersonProfileDTO(string firstName, string secondName, 
            string thirdName, string lastName, enGender gender, 
            string phone, string address) 
            :base(firstName, secondName, thirdName, lastName,gender, phone,address)
        {
           
        }
    }
}
