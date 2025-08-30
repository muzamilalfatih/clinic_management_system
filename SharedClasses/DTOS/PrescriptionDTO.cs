namespace SharedClasses
{
    public class PrescriptionDTO
     {
        public int id { get; set; }
        public int appoinmentId { get; set; }
        public DateTime date { get; set; }
        public string notes { get; set; }
        public PrescriptionDTO(int id, int appoinmentId, DateTime date, string notes)
         {
             this.id = id;
             this.appoinmentId = appoinmentId;
             this.date = date;
             this.notes = notes;
         }
     }
}
