using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Patients
{
    public class UpdatePatientDTO : BasePatientDTO
    {
        public UpdatePatientDTO(int userId, UpdatePatientResquestDTO updatePatientResquestDTO) 
            :base(updatePatientResquestDTO.medicalHistory,updatePatientResquestDTO.allergies)
        {
            this.userId = userId;
        }
        public int userId { get; set; }
    }
}
