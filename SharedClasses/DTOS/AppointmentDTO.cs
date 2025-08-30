namespace SharedClasses
{
    public class AppointmentDTO
     {
        public int id { get; set; }
        public int patientId { get; set; }
        public int doctorId { get; set; }
        public int billId { get; set; }
        public DateTime appointmentTime { get; set; }
        public byte status { get; set; }
        public string notes { get; set; }
        public int? parentAppoinmentId { get; set; }
        public AppointmentDTO(int id, int patientId, int doctorId, int billId, DateTime appointmentTime, byte status, string notes, int? parentAppoinmentId)
         {
             this.id = id;
             this.patientId = patientId;
             this.doctorId = doctorId;
             this.billId = billId;
             this.appointmentTime = appointmentTime;
             this.status = status;
             this.notes = notes;
             this.parentAppoinmentId = parentAppoinmentId;
         }
     }
}
