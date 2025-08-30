using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Patients
{
    public class UpdatePatientResquestDTO : BasePatientDTO
    {
        public  UpdatePatientResquestDTO(string medicalHistory, string allergies) : base(medicalHistory, allergies)
        {
        }
    }
}
