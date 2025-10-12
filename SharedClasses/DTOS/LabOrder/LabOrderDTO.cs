using SharedClasses.Enums;

namespace SharedClasses.DTOS.LabOrder
{
    public class LabOrderDTO
     {
        public LabOrderDTO(int id, int? appointmentId, int? personId, int? testedByTechnicainId, 
            int billId, DateTime date, LabOrderStatus status, string? notes)
        {
            Id = id;
            AppointmentId = appointmentId;
            PersonId = personId;
            TestedByTechnicianId = testedByTechnicainId;
            BillId = billId;
            Date = date;
            Status = status;
            Notes = notes;
        }

        public int Id { get; set; }
        public int? AppointmentId { get; set; }
        public int? PersonId { get; set; }
        public int? TestedByTechnicianId { get; set; }
        public int BillId { get; set; }
        public DateTime Date { get; set; }
        public LabOrderStatus Status { get; set; }
        public string? Notes { get; set; }


      
     }
}
