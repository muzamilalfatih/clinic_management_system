using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Diagnose
{
    public class UpdateDiagnoseDTO
    {
        public UpdateDiagnoseDTO(int id, int appointmentId, string diagnosisCode, string description)
        {
            Id = id;
            AppointmentId = appointmentId;
            DiagnosisCode = diagnosisCode;
            Description = description;
        }

        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string DiagnosisCode { get; set; }
        public string Description { get; set; }
    }
}
