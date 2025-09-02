using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Payment
{
    public class MakePaymentDTO
    {
        public MakePaymentDTO(int billId, int paymentMethodId)
        {
            BillId = billId;
            PaymentMethodId = paymentMethodId;
        }

        public int BillId { get; set; }
        public int PaymentMethodId { get; set; }
    }
}
