using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabOrderResults
{
    public class LabOrderResultDTO
    {
        public LabOrderResultDTO(int id, int labOrderTestId, int labTestParameterId, string result)
        {
            Id = id;
            LabOrderTestId = labOrderTestId;
            LabTestParameterId = labTestParameterId;
            Result = result;
        }

        public int Id { get; set; }
        public int LabOrderTestId { get; set; }
        public int LabTestParameterId { get; set; }
        public string Result { get; set; }
    }
}
