using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabOrderResults
{
    public class AddNewOrderResultDTO
    {
        public AddNewOrderResultDTO(int labOrderTestId, int labTestParameterId, string result)
        {
            LabOrderTestId = labOrderTestId;
            LabTestParameterId = labTestParameterId;
            Result = result;
        }

        public int LabOrderTestId { get; set; }
        public int LabTestParameterId { get; set; }
        public string Result { get; set; }
    }
}
