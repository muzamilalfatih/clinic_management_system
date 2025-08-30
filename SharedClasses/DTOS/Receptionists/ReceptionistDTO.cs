using SharedClasses.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public  class ReceptionistDTO : BaseReceptionistDTO
    {
        public ReceptionistDTO(int id, int userId, int shiftTypeId, DateTime hireDate)
            :base(shiftTypeId, hireDate)
        {
            Id = id;
            UserId = userId;
        }

        public int Id { get; set; }
        public int UserId { get; set; }


    }
}
