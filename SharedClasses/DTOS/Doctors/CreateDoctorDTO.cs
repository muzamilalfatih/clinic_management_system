using SharedClasses.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class CreateDoctorDTO : BaseDoctorDTO
    {
        [JsonConstructor]
        public CreateDoctorDTO(int userId, int specializationId, byte prevExperienceYears,DateTime joinDate, string bio, decimal consultationFee)
            :base( specializationId,  prevExperienceYears, joinDate,  bio,  consultationFee)
        {

            this.userId = userId;
        }
        public int userId {  get; set; }
    }
}
