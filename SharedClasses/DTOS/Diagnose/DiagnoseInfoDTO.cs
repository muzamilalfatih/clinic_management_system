using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Diagnose
{
    public class DiagnoseInfoDTO
    {
        public DiagnoseInfoDTO(int id, int appointmentId, string patientName, DateTime date, string diagnosis, string description)
        {
            Id = id;
            AppointmentId = appointmentId;
            PatientName = patientName;
            Date = date;
            Diagnosis = diagnosis;
            Description = description;
        }

        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public DateTime Date { get; set; }
        public string Diagnosis { get; set; }
        public string Description { get; set; }
    }
}
