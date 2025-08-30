using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTechnician
{
    public class CreateLabTechnicianResponseDTO : BaseLabTechnicianDTO
    {
        public CreateLabTechnicianResponseDTO( CreateLabTechnicianDTO createLabTechnicianDTO)
            :base(createLabTechnicianDTO.DepartmentId,createLabTechnicianDTO.PrevExperienceYears,createLabTechnicianDTO.JoinDate)
        {
        }
    }
}
