using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Receptionists
{
    public  class CreateReceptionistDTO :BaseReceptionistDTO
    {
        public CreateReceptionistDTO(int userId, int shiftTypeId, DateTime hireDate)
            :base(shiftTypeId, hireDate)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
