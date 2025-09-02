using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Doctors
{
    public class UpdateDoctorDTO : BaseDoctorDTO
    {
        
        public UpdateDoctorDTO(int id, int specializationId,byte prevExperienceYears, DateTime joinDate, string bio, decimal consulationFee)
           : base(specializationId, prevExperienceYears, joinDate,bio, consulationFee)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
