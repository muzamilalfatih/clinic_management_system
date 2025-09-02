using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.People
{
    public class UpdatePersonDTO : BasePersonDTO
    {
        public UpdatePersonDTO(int id, string firstName, string secondName, string thirdName, string lastName, enGender gender, string phone, string address)
            : base(firstName, secondName, thirdName, lastName, gender, phone, address)
        {
            this.id = id;
        }

        public int id { get; set; }
    }
}
