namespace SharedClasses
{
    public class PaymentMethodDTO
     {
        public int id { get; set; }
        public string name { get; set; }
        public PaymentMethodDTO(int id, string name)
         {
             this.id = id;
             this.name = name;
         }
     }
}
