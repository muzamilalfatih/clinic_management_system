using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTechnician
{
    public class UpdateLabTechnicianDTO : BaseLabTechnicianDTO
    {
        public UpdateLabTechnicianDTO(int userId, UpdateLabTechnicianRequestDTO updateLabTechnicianRequestDTO)
            : base(updateLabTechnicianRequestDTO.DepartmentId, updateLabTechnicianRequestDTO.PrevExperienceYears, updateLabTechnicianRequestDTO.JoinDate)
        {
            UserId = userId;
        }
        public int UserId { get; set; }
    }
}
