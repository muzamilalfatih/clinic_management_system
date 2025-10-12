using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabOrder
{
    public class AddNewLabOrderDTO
    {
        public AddNewLabOrderDTO(int? appointmentId, int? personId, string? notes)
        {
            AppointmentId = appointmentId;
            PersonId = personId;
            Notes = notes;
        }

        public int? AppointmentId { get; set; }
        public int? PersonId { get; set; }
        public string? Notes { get; set; }
    }
}
