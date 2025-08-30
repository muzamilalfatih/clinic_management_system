using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Doctors
{
    public class CreateDoctorResponseDTO : BaseDoctorDTO
    {
        public CreateDoctorResponseDTO(CreateDoctorDTO createDoctorDTO)
            :base(createDoctorDTO.specializationId, createDoctorDTO.prevExperienceYears, createDoctorDTO.joinDate, createDoctorDTO.bio, createDoctorDTO.consultationFee)
        {
        }
    }
}
