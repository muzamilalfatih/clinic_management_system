using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Bills
{
    public class BillInfoDTO
    {
        public BillInfoDTO(int id, decimal amount, DateTime date, BillStatus status)
        {
            Id = id;
            Amount = amount;
            Date = date;
            Status = status;
        }

        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public BillStatus Status { get; set; }
    }
}
