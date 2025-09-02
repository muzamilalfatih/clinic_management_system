using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Diagnose
{
    public class DiagnoseDTO
    {
        public DiagnoseDTO(int id, int appointmentId, DateTime date, string diagnosisCode, string description)
        {
            Id = id;
            AppointmentId = appointmentId;
            Date = date;
            DiagnosisCode = diagnosisCode;
            Description = description;
        }

        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string DiagnosisCode { get; set; }
        public string Description { get; set; }


    }
}
