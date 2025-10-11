using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Prescription
{
    public class UpdatePrescriptionDTO
    {
        public UpdatePrescriptionDTO(int id, int appointmentId, string notes)
        {
            Id = id;
            AppointmentId = appointmentId;
            Notes = notes;
        }

        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string Notes { get; set; }
    }
}
