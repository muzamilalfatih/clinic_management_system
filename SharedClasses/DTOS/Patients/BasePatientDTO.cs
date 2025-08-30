using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Patients
{
    public class BasePatientDTO
    {
        public BasePatientDTO(string medicalHistory, string allergies)
        {
            this.medicalHistory = medicalHistory;
            this.allergies = allergies;
        }

        public string medicalHistory {  get; set; }
        public string allergies { get; set; }

    }
}
