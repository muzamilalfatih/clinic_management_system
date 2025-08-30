using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.People
{
    public class CreatePersonResponseDTO : BasePersonDTO
    {
        public CreatePersonResponseDTO(CreatePersonDTO createPersonDTO) 
            : base(createPersonDTO.firstName, createPersonDTO.secondName, createPersonDTO.thirdName, createPersonDTO.lastName, createPersonDTO.gender, createPersonDTO.phone, createPersonDTO.address)
        {
        }
    }
}
