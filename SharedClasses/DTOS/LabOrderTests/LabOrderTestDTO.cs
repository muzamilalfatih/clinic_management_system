namespace SharedClasses
{
    public class LabOrderTestDTO
     {
        public int id { get; set; }
        public int labOrderId { get; set; }
        public int labTestId { get; set; }
        public string result { get; set; }
        public string normalRange { get; set; }
        public string unit { get; set; }
        public DateTime? resultDate { get; set; }
        public LabOrderTestStatus status { get; set; }
        public int? testedByTechnicianID { get; set; }
        public LabOrderTestDTO(int id, int labOrderId, int labTestId, string result, string normalRange, string unit, DateTime? resultDate, LabOrderTestStatus status, int? testedByTechnicianID)
         {
             this.id = id;
             this.labOrderId = labOrderId;
             this.labTestId = labTestId;
             this.result = result;
             this.normalRange = normalRange;
             this.unit = unit;
             this.resultDate = resultDate;
             this.status = status;
             this.testedByTechnicianID = testedByTechnicianID;
         }
     }
}
