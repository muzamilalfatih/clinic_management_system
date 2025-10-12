using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabOrder
{
    public class AddNewLabOrderRequestDTO
    {
        public AddNewLabOrderRequestDTO(AddNewLabOrderDTO newlabOrder, List<int> labTestIds)
        {
            NewlabOrder = newlabOrder;
            LabTestIds = labTestIds;
        }

        public AddNewLabOrderDTO NewlabOrder { get; set; }
        public List<int> LabTestIds { get; set; }
    }
}
