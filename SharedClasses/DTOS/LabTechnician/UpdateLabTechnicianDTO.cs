using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTechnician
{
    public class UpdateLabTechnicianDTO : BaseLabTechnicianDTO
    {
        public UpdateLabTechnicianDTO(int id, int userId, int departmentId, byte  prevExperienceYears, DateTime joinDate)
            : base(departmentId, prevExperienceYears, joinDate)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
