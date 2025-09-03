using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTestParameter
{
    public class AddNewLabTestParameterDTO
    {
        public AddNewLabTestParameterDTO(int labTestId, string name, string normalRange, string? unit)
        {
            LabTestId = labTestId;
            Name = name;
            NormalRange = normalRange;
            Unit = unit;
        }

        public int LabTestId { get; set; }
        public string Name { get; set; }
        public string NormalRange { get; set; }
        public string? Unit { get; set; }
    }
}
