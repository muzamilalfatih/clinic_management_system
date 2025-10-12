namespace SharedClasses
{
    public class LabOrderTestDTO
    {
        public int Id { get; set; }
        public int LabOrderId { get; set; }
        public int LabTestId { get; set; }
        public decimal Fee { get; set; }
        public LabOrderTestStatus Status { get; set; }

        public LabOrderTestDTO(int id, int labOrderId, int labTestId, decimal fee, LabOrderTestStatus status)
        {
            Id = id;
            LabOrderId = labOrderId;
            LabTestId = labTestId;
            Fee = fee;
            Status = status;
        }
    }
}
