using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTechnician
{
    public class LabTechnicianProfileDTO
    {
        public LabTechnicianProfileDTO(string department, byte prevExperienceYears, DateTime joinDate)
        {
            Department = department;
            _prevExperienceYears = prevExperienceYears;
            _joinDate = joinDate;
        }

        public string Department {  get; set; }
        public int ExperienceYears => _prevExperienceYears + ((DateTime.Now - _joinDate).Days / 350);
        private int _prevExperienceYears { get; set; }
        private DateTime _joinDate { get; set; }
    }
}
