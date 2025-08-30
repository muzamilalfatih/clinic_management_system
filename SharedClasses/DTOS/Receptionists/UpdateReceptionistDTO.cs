using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Receptionists
{
    public class UpdateReceptionistDTO :BaseReceptionistDTO
    {
        public UpdateReceptionistDTO(int userId, UpdateReceptionistRequestDTO updateReceptionistRequestDTO)
            :base( updateReceptionistRequestDTO.ShiftTypeId, updateReceptionistRequestDTO.HireDate)
        {
            
            UserId = userId;
        }
        public int UserId {  get; set; }
    }
}
