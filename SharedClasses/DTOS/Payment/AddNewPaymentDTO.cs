using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Payment
{
    public class AddNewPaymentDTO
    {
        public AddNewPaymentDTO(MakePaymentDTO makePayment, decimal paidAmont)
        {
            BillId = makePayment.BillId;
            PaidAmont = paidAmont;
            PaymentMethodId = makePayment.PaymentMethodId;
        }

        public int BillId { get; set; }
        public decimal PaidAmont { get; set; }
        public decimal PaymentMethodId { get; set; }
    }
}
