using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTechnician
{
    public class CreateLabTechnicianDTO : BaseLabTechnicianDTO
    {
        public CreateLabTechnicianDTO(int userId , int departmentId, byte prevExperienceYears, DateTime joinDate)
            :base(departmentId, prevExperienceYears, joinDate) 
        {
            UserId = userId;
        }
        public int UserId { get; set; }
    }
}
