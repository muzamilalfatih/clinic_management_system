using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Patients
{
    public class UpdatePatientDTO : BasePatientDTO
    {
        public UpdatePatientDTO(int id, string medicalHistory, string allergies) 
            :base(medicalHistory, allergies)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
