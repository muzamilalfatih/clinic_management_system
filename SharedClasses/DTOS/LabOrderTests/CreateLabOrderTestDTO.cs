using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabOrderTests
{
    public class CreateLabOrderTestDTO
    {
        public CreateLabOrderTestDTO(int labOrderId, int labTestId, string normalRange, string unit)
        {
            this.labOrderId = labOrderId;
            this.labTestId = labTestId;
            this.normalRange = normalRange;
            this.unit = unit;
        }

        public int labOrderId { get; set; }
        public int labTestId { get; set; }
        public string normalRange { get; set; }
        public string unit {  get; set; }

    }
}
