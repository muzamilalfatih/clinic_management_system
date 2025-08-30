using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Patients
{
    public class CreatePatientDTO : BasePatientDTO
    {
        public CreatePatientDTO(int userId, string medicalHistory, string allergies)
            :base(medicalHistory, allergies) 
        {
            this.userId = userId;
        }

        public int userId {  get; set; }
    }
}
