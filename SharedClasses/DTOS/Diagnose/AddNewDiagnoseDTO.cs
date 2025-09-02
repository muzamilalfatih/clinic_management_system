using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Diagnose
{
    public class AddNewDiagnoseDTO
    {
        public AddNewDiagnoseDTO(int appointmentId, string diagnosisCode, string description)
        {
            AppointmentId = appointmentId;
            DiagnosisCode = diagnosisCode;
            Description = description;
        }

        public int AppointmentId { get; set; }
        public string DiagnosisCode { get; set; }
        public string Description { get; set; }
    }
}
