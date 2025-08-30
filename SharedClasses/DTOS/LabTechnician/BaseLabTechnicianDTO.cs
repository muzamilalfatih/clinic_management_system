using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTechnician
{
    public class BaseLabTechnicianDTO
    {
        public BaseLabTechnicianDTO(int departmentId, byte prevExperienceYears, DateTime joinDate)
        {
            DepartmentId = departmentId;
            PrevExperienceYears = prevExperienceYears;
            JoinDate = joinDate;
        }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid id")]
        public int DepartmentId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Must be 1 or more")]
        public byte PrevExperienceYears { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime JoinDate { get; set; }
    }
}
