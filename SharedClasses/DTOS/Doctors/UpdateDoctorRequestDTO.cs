using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Doctors
{
    public class UpdateDoctorRequestDTO : BaseDoctorDTO
    {
        public UpdateDoctorRequestDTO(int specializationId, byte prevExperienceYears, DateTime joinDate, string bio, decimal consulationFee)
            :base(specializationId, prevExperienceYears, joinDate, bio, consulationFee)
        {

        }

    }
}
