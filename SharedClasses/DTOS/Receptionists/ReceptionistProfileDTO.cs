using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Receptionists
{
    public class ReceptionistProfileDTO
    {
        public ReceptionistProfileDTO(string shiftType, DateTime hireDate)
        {
            ShiftType = shiftType;
            HireDate = hireDate;
        }

        public string ShiftType {  get; set; }
        public DateTime HireDate { get; set; }

    }
}
