using SharedClasses.DTOS.PescriptionItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Prescription
{
    public class AddNewPrescriptionDTO
    {
        public AddNewPrescriptionDTO(int appointmentId, string notes)
        {
            AppointmentId = appointmentId;
            Notes = notes;
        }

        public int AppointmentId { get; set; }
        public string Notes { get; set; }
        
    }
}
