using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Doctors
{
    public class UpdateDoctorDTO : BaseDoctorDTO
    {
        public UpdateDoctorDTO(int userId, UpdateDoctorRequestDTO updateDoctorRequestDTO)
            :base(updateDoctorRequestDTO.specializationId,updateDoctorRequestDTO.prevExperienceYears, updateDoctorRequestDTO.joinDate,
                 updateDoctorRequestDTO.bio,updateDoctorRequestDTO.consultationFee)
        {
            this.userId = userId;
        }
        public UpdateDoctorDTO(int userId, int specializationId,byte prevExperienceYears, DateTime joinDate, string bio, decimal consulationFee)
           : base(specializationId, prevExperienceYears, joinDate,bio, consulationFee)
        {
            this.userId = userId;
        }
        public int userId { get; set; }
    }
}
