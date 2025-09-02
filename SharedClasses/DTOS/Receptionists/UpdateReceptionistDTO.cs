using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Receptionists
{
    public class UpdateReceptionistDTO : BaseReceptionistDTO
    {
        public UpdateReceptionistDTO(int id, int shiftTypeId, DateTime hireDate)
            : base(shiftTypeId, hireDate)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
