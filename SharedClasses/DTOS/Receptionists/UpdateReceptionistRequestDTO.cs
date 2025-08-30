using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Receptionists
{
    public class UpdateReceptionistRequestDTO : BaseReceptionistDTO
    {
        public UpdateReceptionistRequestDTO(int shiftTypeId, DateTime hireDate)
            : base(shiftTypeId, hireDate)
        {
        }

    }
}
