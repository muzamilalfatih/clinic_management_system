namespace SharedClasses.DTOS.Bills
{
    public class BillDTO
     {
        public int id { get; set; }
        public float totalAmount { get; set; }
        public DateTime date { get; set; }
        public byte status { get; set; }
        public BillDTO(int id, float totalAmount, DateTime date, byte status)
         {
             this.id = id;
             this.totalAmount = totalAmount;
             this.date = date;
             this.status = status;
         }
     }
}
