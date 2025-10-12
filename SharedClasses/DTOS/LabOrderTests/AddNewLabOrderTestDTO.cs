using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabOrderTests
{
    public class AddNewLabOrderTestDTO
    {   
        public AddNewLabOrderTestDTO(int labTestId, decimal fee)
        {
            LabTestId = labTestId;
            Fee = fee;
        }

        public int LabTestId { get; set; }
       public decimal Fee { get; set; }

    }
}
