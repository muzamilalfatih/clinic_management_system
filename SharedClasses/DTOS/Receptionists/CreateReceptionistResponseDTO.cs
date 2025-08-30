using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Receptionists
{
    public class CreateReceptionistResponseDTO
        :BaseReceptionistDTO
    {
        public CreateReceptionistResponseDTO(int id, CreateReceptionistDTO receptionistDTO)
            :base(receptionistDTO.ShiftTypeId, receptionistDTO.HireDate)
        {
            Id = id;
            UserID = receptionistDTO.UserId;
        }

        public int Id { get; set; }
        public int UserID {  get; set; }
    }
}
