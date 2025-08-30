namespace SharedClasses
{
    public class LabOrderDTO
     {
        public int id { get; set; }
        public int? appointmentId { get; set; }
        public int? patientId { get; set; }
        public int? doctorId { get; set; }
        public int billId { get; set; }
        public DateTime date { get; set; }
        public byte status { get; set; }
        public string note { get; set; }
        public LabOrderDTO(int id, int? appointmentId, int? patientId, int? doctorId, int billId, DateTime date, byte status, string note)
         {
             this.id = id;
             this.appointmentId = appointmentId;
             this.patientId = patientId;
             this.doctorId = doctorId;
             this.billId = billId;
             this.date = date;
             this.status = status;
             this.note = note;
         }
     }
}
