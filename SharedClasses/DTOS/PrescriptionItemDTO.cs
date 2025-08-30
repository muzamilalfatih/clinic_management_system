namespace SharedClasses
{
    public class PrescriptionItemDTO
     {
        public int id { get; set; }
        public int prescriptionId { get; set; }
        public string medicineName { get; set; }
        public string dosage { get; set; }
        public string frequency { get; set; }
        public string duration { get; set; }
        public PrescriptionItemDTO(int id, int prescriptionId, string medicineName, string dosage, string frequency, string duration)
         {
             this.id = id;
             this.prescriptionId = prescriptionId;
             this.medicineName = medicineName;
             this.dosage = dosage;
             this.frequency = frequency;
             this.duration = duration;
         }
     }
}
