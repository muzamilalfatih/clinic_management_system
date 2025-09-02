namespace SharedClasses.DTOS.Payment
{
    public class PaymentDTO
     {
        public int id { get; set; }
        public int billId { get; set; }
        public float paidAmount { get; set; }
        public int paymentMethodId { get; set; }
        public DateTime date { get; set; }
        public PaymentDTO(int id, int billId, float paidAmount, int paymentMethodId, DateTime date)
         {
             this.id = id;
             this.billId = billId;
             this.paidAmount = paidAmount;
             this.paymentMethodId = paymentMethodId;
             this.date = date;
         }
     }
}
