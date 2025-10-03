using SharedClasses.DTOS.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTest
{
    public class AddNewLabTestDTO
    {
        public AddNewLabTestDTO(string name, string description, decimal price, List<AddNewPaymentDTO> parameters)
        {
            Name = name;
            Description = description;
            Price = price;
            Parameters = parameters;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<AddNewPaymentDTO> Parameters { get; set; }
    }
}
