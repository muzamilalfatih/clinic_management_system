using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabTestParameter
{
    public class LabTestParameterDTO
    {
        public LabTestParameterDTO(int id, int labTestId, string name, string normalRange, string? unit)
        {
            Id = id;
            LabTestId = labTestId;
            Name = name;
            NormalRange = normalRange;
            Unit = unit;
        }

        public int Id { get; set; }
        public int LabTestId { get; set; }
        public string Name { get; set; }
        public string NormalRange { get; set; }
        public string? Unit { get; set; }
    }
}
