using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.People
{
    public class UpdatePersonDTO : BasePersonDTO
    {
        public UpdatePersonDTO(int id, UpdatePersonRequestDTO updatePersonRequestDTO)
            : base(updatePersonRequestDTO.firstName, updatePersonRequestDTO.secondName, updatePersonRequestDTO.thirdName, updatePersonRequestDTO.lastName, updatePersonRequestDTO.gender, updatePersonRequestDTO.phone, updatePersonRequestDTO.address)
        {
            this.id = id;
        }

        public int id { get; set; }
    }
}
