using SharedClasses.DTOS.LabTechnician;

namespace SharedClasses
{
    public class LabTechnicianDTO : BaseLabTechnicianDTO
     {
        public int id { get; set; }
        public int userId { get; set; }
        public int deparmentId { get; set; }
        public LabTechnicianDTO(int id, int userId, int departmentId, byte prevExperienceYears, DateTime joinDate)
            : base(departmentId, prevExperienceYears, joinDate) 
         {
             this.id = id;
             this.userId = userId;
         }
     }
}
