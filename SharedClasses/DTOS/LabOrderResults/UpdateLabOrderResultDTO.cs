using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabOrderResults
{
    public class UpdateLabOrderResultDTO : LabOrderResultDTO
    {
        public UpdateLabOrderResultDTO(int id, int labOrderTestId, int labTestParameterId, string result) 
            : base(id, labOrderTestId, labTestParameterId, result)
        {
        }
    }
}
