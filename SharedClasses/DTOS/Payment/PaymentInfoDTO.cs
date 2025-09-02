using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Payment
{
    public class PaymentInfoDTO
    {
        public PaymentInfoDTO(int id, int billId, decimal paidAmount, string paymentMethod, DateTime date)
        {
            Id = id;
            BillId = billId;
            PaidAmount = paidAmount;
            PaymentMethod = paymentMethod;
            Date = date;
        }

        public int Id { get; set; }
        public int BillId { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime Date { get; set; }
    }
}
