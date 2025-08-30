using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTechnician
{
    public class UpdateLabTechnicianRequestDTO :BaseLabTechnicianDTO
    {
        public UpdateLabTechnicianRequestDTO(int departmentId, byte prevExperienceYears, DateTime joinDate)
            :base(departmentId, prevExperienceYears, joinDate)
        {

        }
    }
}
