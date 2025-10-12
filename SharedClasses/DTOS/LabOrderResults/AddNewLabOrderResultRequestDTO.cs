using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabOrderResults
{
    public class AddNewLabOrderResultRequestDTO
    {
        public AddNewLabOrderResultRequestDTO(int labOderTestId, List<AddNewOrderResultDTO> newOrderResults)
        {
            LabOderTestId = labOderTestId;
            NewOrderResults = newOrderResults;
        }

        public int LabOderTestId { get; set; }
        public List<AddNewOrderResultDTO> NewOrderResults { get; set; }
    }
}
